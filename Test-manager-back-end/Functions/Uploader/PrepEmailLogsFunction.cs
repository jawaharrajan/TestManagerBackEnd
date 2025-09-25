using TestManager.Domain.DTO.Uploader;
using TestManager.Functions.Common;
using TestManager.Service.Helper;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestManagerBackEnd.Functions.Uploader;

public class PrepEmailLogsFunction(IPrepEmailLogService prepEmailService, ILogger<PrepEmailLogsFunction> logger) : BaseFunction(logger)
{    
    [Function("UploaderGetPrepEmailLogsByPatientId")]
    public async Task<IActionResult> UploaderGetPrepEmailLogsByPatientId([HttpTrigger(AuthorizationLevel.Function, "get",Route = "uploader/{patientId}/prepEmailLogs")] HttpRequest req,
        int? patientId)
    {        
        if (!patientId.HasValue)
        {
            logger.LogWarning("PatientId must be present to return Advice logs");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid payload", false));
        }

        logger.LogInformation($"Fetching all Prep Email logs for for Patient {patientId}");

               return await ExecuteSafeAsync(
                async () =>
                {
                    var adviceLogs = await prepEmailService.GetEmailLogsByPatientId(patientId.Value) ??
                        throw new KeyNotFoundException($"Patient details for Id: {patientId} Not found"); ;
                    return adviceLogs;
                }, $"Get Prep Email logs"
            );       
    }    
}