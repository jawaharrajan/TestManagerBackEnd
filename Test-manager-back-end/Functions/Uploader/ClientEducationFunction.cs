using TestManager.Domain.DTO.Uploader;
using TestManager.Functions.Common;
using TestManager.Service.Helper;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

namespace TestManagerBackEnd.Functions.Uploader;

public class ClientEducationFunction(IClientEducationService clientEducationService, ILogger<ClientEducationFunction> logger) : BaseFunction(logger)
{
    [Function("UploaderGetClientEducationByPatientId")]
    public async Task<IActionResult> GetClientEducationByPatientId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/{patientId}/clientEducation")] 
        HttpRequest req, int patientId)
    {
        logger.LogInformation($"Fetching all Education Materials for Patient {patientId}");

        var result = await clientEducationService.GetClientEducationByPatientId(patientId);

        logger.LogInformation($"Retrieved {result.Count()} Materials for Patient: {patientId}");

        return new OkObjectResult(result);
    }

    [Function("UploaderUpdateEducationMaterials")]
    public async Task<IActionResult> UploaderGetResourceEducationMaterials([HttpTrigger(AuthorizationLevel.Function, "post", Route = "uploader/{patientId}/clientEducation")]
        HttpRequest req, int patientId)
    {
       
        var clientEducationDTOs = await req.ReadFromJsonAsync<List<PrepClientEducationDTO>>();
        if (clientEducationDTOs is null)
        {
            logger.LogWarning("UploaderUpdateEducationMaterials: received empty payload");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid payload", false));
        }

        logger.LogInformation($"Updating Client Education Materials");

        return await ExecuteSafeAsync(async () =>
                {
                    return await clientEducationService.UpsertClientEducationByPatientId(patientId, clientEducationDTOs);
                }, $"Client Education Updated");
            
    }
}