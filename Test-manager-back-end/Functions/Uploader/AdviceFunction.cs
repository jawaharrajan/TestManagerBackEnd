using TestManager.Domain.DTO.Uploader;
using TestManager.Functions.Common;
using TestManager.Service.Helper;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestManagerBackEnd.Functions.Uploader;

public class AdviceFunction(IAdviceService adviceService, ILogger<AdviceFunction> logger) : BaseFunction(logger)
{    
    [Function("UploaderGetAdviceByPatientId")]
    public async Task<IActionResult> UploaderGetAdviceByPatientId([HttpTrigger(AuthorizationLevel.Function, "get",Route = "uploader/advice/{patientId}")] HttpRequest req,
        int? patientId)
    {        
        if (!patientId.HasValue)
        {
            logger.LogWarning("PatientId must be present to return Advice logs");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid payload", false));
        }

        logger.LogInformation($"Fetching all Advice logs for for Patient {patientId}");

        return await ExecuteSafeAsync(
         async () =>
         {
             var adviceLogs = await adviceService.GetAdviceByPatientId(patientId.Value) ??
                 throw new KeyNotFoundException($"Patient details for Id: {patientId} Not found"); ;
             return adviceLogs;
         }, $"Get Advice logs");
    }    
}