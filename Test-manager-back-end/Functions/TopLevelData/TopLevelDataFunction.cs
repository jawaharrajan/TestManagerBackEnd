using TestManagerBackEnd.Functions.Radiology;
using TestManager.Service.Helper;
using TestManager.Service.TopLevelFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.TopLevelData
{
    public class TopLevelDataFunction(ITopLevelDataService topLevelDataService , ILogger<AppointmentStatusFunction> logger) : BaseFunction(logger)
    {
       
        [Function("TopLevelData")]
        public async Task<IActionResult> GetTopLevelData([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            logger.LogInformation("Fetching all data for  Top level filters");
            return await ExecuteSafeAsync(
                async () =>
                {
                    var topLevelData = await topLevelDataService.GetTopLevelData();
                    return topLevelData;
                }, $"Retrieve Top Level Data");
        }
    }
}
