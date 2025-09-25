using TestManager.Functions.Common;
using TestManager.Service.Helper;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestManagerBackEnd.Functions.Uploader;

public class AccuroLabObservationResultsActivityFunction(
    IAccuroLabObservationResultsActivityService accuroLabObsResultsActivityService, 
    ILogger<AccuroLabObservationResultsActivityFunction> logger) : BaseFunction(logger)
{

    [Function("UploadeGetAccuroLabObsResultsActivityLogsByPatientId")]
    public async Task<IActionResult> GetAccuroLabObsResultsActivityLogsByPatientId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/{patientId:int}/accuroActivityLogs")] HttpRequest req,
        int? patientId)
    {
        if (!patientId.HasValue)
        {
            logger.LogWarning("PatientId must be present to return Accuro Lab Obs Result Activity logs");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid payload", false));
        }

        logger.LogInformation($"Fetching all Accuro Lab Obs Result Activity logs for for Patient {patientId}");

        return await ExecuteSafeAsync(
         async () =>
         {
             var adviceLogs = await accuroLabObsResultsActivityService.GetAccuroLabObsResultsActivityLogsByPatientId(patientId.Value) ??
                 throw new KeyNotFoundException($"Accuro Lab Obs Result Activity for Patient Id: {patientId} Not found"); ;
             return adviceLogs;
         }, $"Get Accuro Lab Obs Result Activity logs"
     );
    }
}