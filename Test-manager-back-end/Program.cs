using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using TestManager.DataAccess.Repository.AtivityLog;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Notes;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.DataAccess.Repository.Shared;
using TestManager.DataAccess.Repository.TopLevelFilters;
using TestManager.DataAccess.Repository.Uploader;
using TestManager.DataAccess.Repository.Users;
using TestManager.Service;
using TestManager.Service.ActivityLog;
using TestManager.Service.EmailService;
using TestManager.Service.EventHubservices;
using TestManager.Service.TopLevelFilter;
using TestManager.Service.Uploader;
using TestManager.Interfaces;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TestManager.DataAccess;
using TestManager.Service.UserContext;
using TestManager.DataAccess.Repository.DBSynchronization;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

//builder.Services.Configure<FilterConfigOptions>(options =>
//{
//    string? rawValues = builder.Configuration["FilterPrecedence"];

//    options.FilterPrecedence = rawValues is not null && !string.IsNullOrWhiteSpace(rawValues)
//        ? JsonSerializer.Deserialize<List<string>>(rawValues) ?? []
//        : [];
//});

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Get AppointmentRules Timer value:
string? appointRulesTimeValue = builder.Configuration["AppointmentRulesSchedule"];
Console.WriteLine($"Appointment Rules Timer Value: {appointRulesTimeValue}");

// Application Insights Connection String (From Azure)
var appInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

if (!string.IsNullOrEmpty(appInsightsConnectionString))
{
    Console.WriteLine($"Application Insights connection string Loaded: {appInsightsConnectionString}");

    builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, options) =>
    {
        module.EnableSqlCommandTextInstrumentation = true; // Tracks SQL Queries
    });

    Serilog.Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "testclient Clinical Manager") // Adds global properties
        .MinimumLevel.Information()
        //.WriteTo.Console()
        // DOMAIN EVENTS ONLY (filtered by IsDomainEvent)
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(evt => evt.Properties.ContainsKey("IsDomainEvent"))
            .WriteTo.ApplicationInsights(
                connectionString: appInsightsConnectionString,
                telemetryConverter: TelemetryConverter.Events
            )
        )
        .CreateLogger();

    // **Remove Default ILogger Providers**
    builder.Services.RemoveAll<Microsoft.Extensions.Logging.ILoggerProvider>();

    // Add Serilog to Logging Services
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders(); // Remove default providers
        loggingBuilder.AddSerilog(Serilog.Log.Logger);
    });

    Serilog.Log.Information("Application Insights logger initialized");
}
else
{
    // Setup Serilog Configuration
    Serilog.Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
        .Enrich.FromLogContext()
        .CreateLogger();

    // Add Serilog to Logging Services
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders(); // Remove default providers
        loggingBuilder.AddSerilog(Serilog.Log.Logger);
    });
    Serilog.Log.Information("Logging to text file");
}

// Get Connection String (From Local or Azure Key Vault)
string connectionString= string.Empty;

if (builder.Environment.IsDevelopment())
{
    // Use local connection string from user secrets
    connectionString = builder.Configuration.GetConnectionString("testclient-cms");
    Serilog.Log.Information("Using Local Database Connection String.");
    Console.WriteLine($"Connection String Loaded user secret: {connectionString}");
}
else
{
    //#region - For Key vault when set up 
    ////Use Azure Key Vault to get the connection string
    var keyVaultUri = builder.Configuration["AZURE_KEY_VAULT_URL"];
    var KeyVaultSecretKey = builder.Configuration["Azure_KeyVault_Secret_Key"];
    if (string.IsNullOrEmpty(keyVaultUri))
    {
        throw new Exception("AZURE_KEY_VAULT_URL is not set in App Settings.");
    }

    var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
    var secret = await client.GetSecretAsync(KeyVaultSecretKey); // Ensure secret name matches
    connectionString = secret.Value.Value;
    Serilog.Log.Information("Using Azure Key Vault Database Connection String.");
    Console.WriteLine($"Connection String Loaded from KV {connectionString}");
    //#endregion

    #region  -- use  environment Variable for DB Conn string
    //connectionString = builder.Configuration["testclient-cms"];
    //Console.WriteLine($"Connection String Loaded env variable: {connectionString}");
    #endregion
}

/*
This is a conditional SSL bypass for internal testclient IP
 */
builder.Services.AddHttpClient("testclientConditionalSSLCertBypass")
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
            {
                var bypassHosts = builder.Configuration["testclientSSLByPassIPS"]?
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? Array.Empty<string>();                
                return bypassHosts.Contains(request.RequestUri.Host) ||
                       errors == System.Net.Security.SslPolicyErrors.None;
            }
        });


builder.Services.AddHttpClient();

// Register Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString)
    .EnableSensitiveDataLogging());

builder.Services.AddScoped(typeof(IGenericRepository<, >), typeof(GenericRepository<, >));
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ITransactionItemRepository, TransactionItemRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentStatusRepository, AppointmentStatusRepository>();
builder.Services.AddScoped<IAppointmentStatusService, AppointmentStatusService>();
builder.Services.AddScoped<IAndrologistRepository, AndrologistRepository>();
builder.Services.AddScoped<IAndrologistService, AndrologistService>();
builder.Services.AddScoped<IDICOMModalityRepository, DICOMModalityRepository>();
builder.Services.AddScoped<IDICOMModalityService, DICOMModalityService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISequenceRepository, SequenceRepository>();
builder.Services.AddScoped<IAppointmentRuleService, AppointmentRuleService>();
builder.Services.AddScoped<IAppointmentRuleRepository, AppointmentRuleRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentTypeRepository, AppointmentTypeRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ILetterRepository, LetterRepository>();
builder.Services.AddScoped<IPrepClientEducationRepository, PrepClientEducationRepository>();
builder.Services.AddScoped<IPrepEmailLogRepository, PrepEmailLogRepository>();
builder.Services.AddScoped<IAccuroObservationResultsActivityRepository, AccuroLabObservationResultsActivityRepository>();

builder.Services.AddScoped<ILetterService, LetterService>();
builder.Services.AddScoped<IClientEducationService, ClientEducationService>();
builder.Services.AddScoped<ITopLevelDataService, TopLevelDataService>();
builder.Services.AddScoped<IApplyAppointmentRulesService, ApplyAppointmentRulesService>();
builder.Services.AddScoped<IApplyAppointmetRules, ApplyAppointmentRules>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IProcessEventHubMessageService, ProcessEventHubMessageService>();
builder.Services.AddScoped<ITableProcessor, AndrologistProcessor>();
builder.Services.AddScoped<ITableProcessor, AppointmentRulesProcessor>();
builder.Services.AddScoped<ITableProcessor, DicommModalityProcessor>();
builder.Services.AddScoped<ITableProcessor, EntityStatusProcessor>();
builder.Services.AddScoped<ITableProcessor, ProductProcessor>();
builder.Services.AddScoped<ITableProcessor, TransactionItemProcessor>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IAccuroLabObservationResultsActivityService, AccuroLabObservationResultsActivityService>();
builder.Services.AddScoped<INurseCommunicationTypeRepository, NurseCommunicationTypeRepository>();
builder.Services.AddScoped<INurseCommunicationService, NurseCommunicationService>();
builder.Services.AddScoped<IPrepReportingTeamRepository, PrepReportingTeamRepository>();
builder.Services.AddScoped<IPrepReportingTeamService, PrepReportingTeamService>();
builder.Services.AddScoped<IPrepResourceCategoryService, PrepResourceCategoryService>();
builder.Services.AddScoped<IPrepResourceRepository, PrepResourceRepository>();
builder.Services.AddScoped<IPrepTemplateRepository, PrepTemplateRepository>();
builder.Services.AddScoped<IPrepTemplateService, PrepTemplateService>();
builder.Services.AddScoped<IPrepEducationMaterialRepository, PrepEducationMaterialRepository>();
builder.Services.AddScoped<IPrepEducationMaterialService, PrepEducationMaterialService>();
builder.Services.AddScoped<IAccuroObservationRepository, AccuroObservationRepository>();
builder.Services.AddScoped<IAccuroObservationService, AccuroObservationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAdviceRepository, AdviceRepository>();
builder.Services.AddScoped<IAdviceService, AdviceService>();
builder.Services.AddScoped<IPrepEmailLogService, PrepEmailLogService>();
builder.Services.AddScoped<ICMS_SyncTrackingRepository, CMS_SyncTrackingRepository>();
builder.Services.AddTransient<EnrichUserContextMiddleware>();
builder.UseMiddleware<EnrichUserContextMiddleware>();




using var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Clinic Manager API Services on Azure Function App started successfully at {Time}", DateTime.UtcNow);


builder.Build().Run();
