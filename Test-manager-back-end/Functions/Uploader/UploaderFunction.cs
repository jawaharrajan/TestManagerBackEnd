using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;
using TestManagerBackEnd.Functions.Radiology;
using TestManager.Service.Helper;
using TestManager.Service;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Uploader
{
    public class UploaderFunction(ILetterService letterService, IAccuroObservationService accuroObservationService, ILogger<UploaderFunction> logger) : BaseFunction(logger)
    {      

        [Function("UploaderAddLetter")]
        public async Task<IActionResult> UploaderAddLetter([HttpTrigger(AuthorizationLevel.Function, "post" , Route= "uploader/letter")] HttpRequest req)
        {            
            var letter = await req.ReadFromJsonAsync<PrepLetterDTO>();
            if (letter is null)
            {
                logger.LogWarning("UploaderAddLetter: received empty payload");
                return new BadRequestObjectResult(new ApiResponse<string>("Invalid payload", false));
            }

            logger.LogInformation($"Add a new Letter");
            return await ExecuteSafeAsync(
                async () =>
                {
                    var letterId = await letterService.AddLetter(letter);
                    if (letterId <= 0) throw new Exception($"Unable to add new Letter");
                    return letterId;
                }, $"New Letter Added"
            );
        }

        [Function("UploaderGetLetterById")]
        public async Task<IActionResult> GetLetterById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/letter/{letterId}")] HttpRequest req, int? letterId)
        {
            if (letterId.HasValue)
            {
                return await ExecuteSafeAsync(
                        async () =>
                        {
                            return await letterService.GetLetterById(letterId.Value);
                        }, $"Retrieving Letter {letterId}"
                    );
            }
            else
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Letter Id is required.", false));
        }

        [Function("UploaderGetAccuroUploadedLogs")]
        public async Task<IActionResult> GetAccuroUploadedLogs([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/{patientId}/logs")] HttpRequest req, int? patientId)
        {
            if (patientId.HasValue)
            {
                return await ExecuteSafeAsync(
                        async () =>
                        {
                            return await accuroObservationService.GetPatientUploadedLogs(patientId.Value);
                        }, $"Get Logs for {patientId}"
                    );
            }
            else
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Patient Id is required.", false));
        }

        [Function("UploaderRemoveLetter")]
        public async Task<IActionResult> RemoveLetter([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "uploader/letter/{letterId}")] HttpRequest req, int? letterId)
        {
            if (letterId.HasValue)
            {
                return await ExecuteCommandAsync(
                        async () =>
                        {
                            await letterService.RemoveLetter(letterId.Value);
                        }, $"Removed Letter {letterId}"
                    );
            }
            else
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Letter Id is required.", false));
        }

        [Function("UploaderGetLettersByPatientIdLetterTypeId")]
        public async Task<IActionResult> GetLettersByPatientIdLetterTypeId([HttpTrigger(AuthorizationLevel.Function, "get", 
            Route = "uploader/patient/{patientId}/lettertype/{letterTypeId}")] HttpRequest req, int? patientId, int? letterTypeId)
        {
            if (patientId.HasValue && letterTypeId.HasValue)
            {
                return await ExecuteSafeAsync(
                        async () =>
                        {
                            return await letterService.GetLettersByPatientIdLetterTypeId(patientId.Value, letterTypeId.Value);
                        }, $"Retrieving Letters by PatientId {patientId} and Lettet TypeId {letterTypeId}"
                    );
            }
            else
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: PatientId and Letter Type Id is required.", false));
        }
    }
}
