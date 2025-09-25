using TestManager.Domain.DTO.ActivityLog;
using TestManager.Service.Helper;
using TestManager.Service.ActivityLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.ActivityLog;

public class ActivityLogFunction(IActivityLogService activityLogService, ILogger<ActivityLogFunction> logger) : BaseFunction(logger)
{

    [Function("GetActivityLog")]
    public async Task<IActionResult> GetActivityLog([HttpTrigger(AuthorizationLevel.Function, "get", Route ="activitylogs")] HttpRequest req)
    {
        //// EnrichLoggingFromRequest(req, enricher);

        var filter = new ActivityLogFilterDto();
        if (req.QueryString.HasValue)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
            filter = new ActivityLogFilterDto
            {
                InstanceId = int.TryParse(query["InstanceId"], out var i) ? i : 0,
                EntityTypeId = int.TryParse(query["EntityTypeId"], out var e) ? e : 0,
                SearchTerm = query["searchTerm"],
                Page = int.TryParse(query["page"], out var p) ? p : 1,
                PageSize = int.TryParse(query["pageSize"], out var ps) ? ps : 20
            };
        }

        logger.LogInformation($"Fetching all Activity Logs for Id: {filter.InstanceId}, " +
            $"EntityTypeId: {filter.EntityTypeId}, SearchTerm: {filter.SearchTerm ?? "NA"}");

        return await ExecutePagedAsync<ActivityLogDTO>(
            async () =>
            {
                var (data, total) = await activityLogService.GetActivityLogsAsync(filter);
                return (data, total);
            },
            filter.Page, filter.PageSize, $"Fetched all Activity Logs for Id: {filter.InstanceId}, " +
            $"EntityTypeId: {filter.EntityTypeId}, SearchTerm: {filter.SearchTerm ?? "NA"}"
        );
    }

    [Function("AddActivityLog")]
    public async Task<IActionResult> AddActivityLog([HttpTrigger(AuthorizationLevel.Function, "post", Route = "activitylogs")] HttpRequest req)
    {
        //// EnrichLoggingFromRequest(req, enricher);
        var log = await req.ReadFromJsonAsync<ActivityLogDTO>();
        if (log is null || log.EntityTypeId == 0 || log.EntityAction == string.Empty)
        {
            logger.LogWarning("AddActivityLog: received empty payload");
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: EntityTypeId or EntityAction cannot be null or empty.", false));
        }

        logger.LogInformation($"Add a new Activtiy Log for EntityType: {log.EntityTypeId}, By:{log.UserEmail}");
        return await ExecuteSafeAsync<bool>(async () => {
            await activityLogService.AddActivityLog(log);
            return true;
        }, $"Note sent succesfully to testclient API");
    }


    //[Function("GetActivityLogByClient")]
    //public async Task<IActionResult> GetActivityLogByClient([HttpTrigger(AuthorizationLevel.Function, "get", Route = "activitylogs")] HttpRequest req)
    //{
    //    // EnrichLoggingFromRequest(req, enricher);

    //    var filter = new ActivityLogFilterDto();
    //    if (req.QueryString.HasValue)
    //    {
    //        var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
    //        filter = new ActivityLogFilterDto
    //        {                
    //            SearchTerm = query["searchTerm"],
    //            Page = int.TryParse(query["page"], out var p) ? p : 1,
    //            PageSize = int.TryParse(query["pageSize"], out var ps) ? ps : 20
    //        };
    //    }

    //    logger.LogInformation($"Fetching all Activity Logs for {filter.SearchTerm}");

    //    return await ExecutePagedAsync<ActivityLogDTO>(
    //        async () =>
    //        {
    //            var (data, total) = await activityLogService.GetActivityLogByClientAsync(filter);
    //            return (data, total);
    //        },
    //        filter.Page, filter.PageSize, logger, $"Get all Activity Logs for {filter.SearchTerm}"
    //    );
    //}

    //[Function("GetActivityLogByAppointment")]
    //public async Task<OkObjectResult> GetActivityLogByAppointment([HttpTrigger(AuthorizationLevel.Function, "get", Route = "activitylogs/{id}")] 
    //    HttpRequest req, int id)
    //{
    //    // EnrichLoggingFromRequest(req, enricher);

    //    logger.LogInformation("Fetching All Activity Logs by Appointment");

    //    var activityLogs = await activityLogService.GetActivityLogByAppointmentAsync(id);            
    //    logger.LogInformation($"Retrieved {activityLogs.Count} ActivityLogs for Appointment {id}");
    //    return new OkObjectResult(activityLogs);
    //}
}