using TestManager.Domain.DTO;
using TestManager.Service.Helper;
using TestManager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Radiology
{
    public class DICOMModalityFunction(IDICOMModalityService dicomModalityservice,ILogger<DICOMModalityFunction> logger) : BaseFunction(logger)
    {
        [Function("GetDICOMModality")]        
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route ="dicommodality")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Fetching all DICOMModality");

            var filter = new DICOMModalityFilterDTO();
            if (req.QueryString.HasValue)
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
                filter = new DICOMModalityFilterDTO
                {
                    RoomCode = query["roomCode"],
                    ModalityCode = query["modalityCode"],
                    SearchTerm = query["searchTerm"],
                };
            }

            var diccomModality = await dicomModalityservice.GetDICOMModalityAsync(filter);

            logger.LogInformation($"Retrieved {diccomModality.Count()} Modalities");
            return new OkObjectResult(diccomModality);

        }

        [Function("AddDICOMModality")]        
        public async Task<IActionResult> AddDICOMModality([HttpTrigger(AuthorizationLevel.Function, "post", Route = "dicommodality")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Adding a new DICOMModality");

            var modality = await req.ReadFromJsonAsync<DICOMModalityDTO>();

            if (modality is null || string.IsNullOrWhiteSpace(modality.StudyDescription) || string.IsNullOrEmpty(modality.RoomCode))
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: DICOMModality cannot be null. DICOMModality details missing", false));

            return await ExecuteSafeAsync(
                 async () => 
                 {
                     var result = await dicomModalityservice.AddDICOMModality(modality);                       
                     return result;
                 }, "AddDICOMModality - DICOM Modality added successfully"
             );            
        }

        [Function("UpdateDICOMModality")]
        public async Task<IActionResult> UpdateDICOMModality([HttpTrigger(AuthorizationLevel.Function, "put", Route = "dicommodality")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Adding a new DICOMModality");

            var modality = await req.ReadFromJsonAsync<DICOMModalityDTO>();

            if (modality is null || string.IsNullOrWhiteSpace(modality.StudyDescription) || string.IsNullOrEmpty(modality.RoomCode))
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: DICOMModality cannot be null. DICOMModality details missing", false));

            return await ExecuteSafeAsync(
                async () => 
                {
                    var result = await dicomModalityservice.UpdateDICOMModality(modality)
                        ?? throw new KeyNotFoundException($"Modality with Id: {modality.ModalityId} Not found");
                    return result;
                }, "UpadateDICOMModality - DICOM Modality updated successfully"
             );
        }

        [Function("DeleteDICOMModality")]
        public async Task<IActionResult> DeleteDICOMModality([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "dicommodality/{Id}")] HttpRequest req,
        int Id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            if (Id <= 0)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: DICOMModalityId is required.", false));
            }
            
            return await ExecuteSafeAsync(
                async () =>
                {
                    var deleted = await dicomModalityservice.DeleteDICOMModality(Id);
                    if (!deleted)
                    {
                        throw new KeyNotFoundException($"DICOMModality with ID {Id} not found.");
                    }
                    // Fetch updated list after successful delete
                    var remaining = await dicomModalityservice.GetDICOMModalityAsync();
                    return remaining;
                }, $"Deleted DIOMMOdality with Id: {Id} successfully"
            );
        }
    }
}
