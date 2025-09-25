using TestManager.Domain.DTO.Uploader;
using TestManagerBackEnd.Functions.Radiology;
using TestManager.Service.Helper;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Uploader
{
    public class NurseCommunicationFunction(INurseCommunicationService nurseCommunicationService, ILogger<NurseCommunicationFunction> logger) : BaseFunction(logger)
    {

        [Function("UploaderGetNurseCommunication")]
        public async Task<IActionResult> UploaderGetNurseCommunication([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/nursecommunication")] HttpRequest req)
        {
            logger.LogInformation("Fetching all Nurse Communication Types");

            var result = await nurseCommunicationService.GetNurseCommunicationTypeAsync();

            logger.LogInformation($"Retrieved {result.Count} Nurse Communication types");

            return new OkObjectResult(result);
        }

        [Function("UploaderAddNurseCommunication")]
        public async Task<IActionResult> AddNurseCommunication([HttpTrigger(AuthorizationLevel.Function, "post", Route = "uploader/nursecommunication")] HttpRequest req)
        {
            logger.LogInformation("Adding new Uploader Nurse Communication");

            var nurseCommunicationDTO = await req.ReadFromJsonAsync<NurseCommunicationTypeDTO>();
            if (nurseCommunicationDTO is null || string.IsNullOrEmpty(nurseCommunicationDTO.Description))
            { 
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Nurse Communication cannot be null. Nurse Communication details missing", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var id = await nurseCommunicationService.AddNurseCommunicationTypeAsync(nurseCommunicationDTO);
                    return id;
                }, "Adding Uploader Nurse Communication"
            );
        }

        [Function("UploaderUpdateNurseCommunication")]
        public async Task<IActionResult> UpdateNurseCommunication([HttpTrigger(AuthorizationLevel.Function, "put", Route = "uploader/nursecommunication")] HttpRequest req)
        {
            logger.LogInformation("Updating Nurse Communication Type");            

            var nurseCommunicationDTO = await req.ReadFromJsonAsync<NurseCommunicationTypeDTO>();
            if (nurseCommunicationDTO is null || string.IsNullOrEmpty(nurseCommunicationDTO.Description))
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Nurse Communication cannot be null. Nurse Communication details missing", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var id = await nurseCommunicationService.UpdateNurseCommunicationTypeAsync(nurseCommunicationDTO)
                     ?? throw new KeyNotFoundException($"Nurse Communication Type with Id: {nurseCommunicationDTO.NurseCommunicationTypeId} Not found");
                    return id;
                }, "Updating Nurse Communication Type"
            );
        }

        [Function("UploaderDeleteNurseCommunication")]
        public async Task<IActionResult> DeleteNurseCommunication([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "uploader/nursecommunication/{id}")] HttpRequest req,
            int id)
        {
            logger.LogInformation("Updating Uploader Nurse Communication Type");

            if (id <= 0)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Nurse Communication Type Id is required.", false));
            }
            return await ExecuteSafeAsync(
                async () =>
                {
                    var deleted = await nurseCommunicationService.DeleteNurseCommunicationTypeAsync(id);
                    if (!deleted)
                    {
                        throw new KeyNotFoundException($"Nurse Communication Type  {id} not found.");
                    }
                    // Fetch updated list after successful delete
                    var remaining = await nurseCommunicationService.GetNurseCommunicationTypeAsync();
                    return remaining;
                }, $"Deleted Uploader Nurse Communnication Type with Id: {id} successfully"
            );
        }
    }
}
