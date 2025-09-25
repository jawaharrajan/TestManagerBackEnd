using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;
using TestManager.Enum;
using TestManager.Functions.Common;
using TestManager.Service.Helper;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestManagerBackEnd.Functions.Uploader;

public class AccuroObservationsFunction(IAccuroObservationService accuroObservationService, ILogger<ClientEducationFunction> logger) : BaseFunction(logger)
{
    [Function("UploaderGetAccuroObservations")]
    public async Task<IActionResult> GetAccuroObservations([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/accuroObservations")]
        HttpRequest req)
    {
        logger.LogInformation($"Attempting to fetch Accuro Observations Filters");
        if (!req.QueryString.HasValue)
        {
            logger.LogWarning("UploaderGetAccuroObservations: Invalid Filter");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid Filter", false));
        }
        var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
        if (!byte.TryParse(query["resultStatus"], out byte resultStatus))
        {
            logger.LogWarning("UploaderAccuroObservationsDropdowns: Invalid Status Filter");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid Status Filter", false));
        }
       
        AccuroObservationFilterDto filter = new()
        {
            ResultStatus = (AccuroObservationResultStatus)resultStatus,
            SearchTerm = query["searchTerm"]

        };

        if (int.TryParse(query["externalLab"], out int externalLab)) 
        {
            filter.ExternalLab = externalLab;
        }
        if (bool.TryParse(query["pediatric"], out bool pediatric))
        {
            filter.Pediatric = pediatric;
        }
        if (int.TryParse(query["reviewedPhysician"], out int reviewedPhysician))
        {
            filter.ReviewedPhysician = reviewedPhysician;
        }
        if (DateTime.TryParse(query["reviewedDate"], out var reviewedDate))
        {
            filter.ReviewedDate = reviewedDate;
        }


        return await ExecutePagedAsync<AccuroLabPatientCollectionDTO>(
               async () =>
               {
                   var (data, total) = await accuroObservationService.GetAccuroObservations(filter);
                   return (data, total);
               }, filter.Page, filter.PageSize, $"Retrieved Accuro Observations Groups for {query["reviewedDate"]}"
           );

    }

    [Function("UploaderGetPatientResultsByReviewDate")]
    public async Task<IActionResult> GetPatientResultsByReviewDate([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/{patientId}/accuroObservations")]
        HttpRequest req, int patientId)
    {
        logger.LogInformation($"Attempting to fetch Accuro Observations Filters");
        if (!req.QueryString.HasValue || patientId <= 0)
        {
            logger.LogWarning("UploaderGetPatientResultsByReviewDate: Invalid Filter");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid Filter", false));
        }
        var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
        var orderIds = query["orders"]?.Split(',').Select(x => x.Trim());
        if (orderIds == null || !orderIds.Any())
        {
            logger.LogWarning("UploaderGetPatientResultsByReviewDate: Invalid Order Id");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid Order Id", false));
        }

        return await ExecuteSafeAsync(
            async () =>
            {
                return await accuroObservationService.GetPatientOrdersResults(patientId, orderIds);
            }, $"Retrieved Accuro Observations Groups for {patientId}"
        );
    }

    [Function("UploaderAccuroObservationsDropdowns")]
    public async Task<IActionResult> GetDropdownsByReviewDate([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/accuroObservations/dropdowns")] 
        HttpRequest req)
    {
        logger.LogInformation($"Attempting to fetch Accuro Observations Filters");
        if (!req.QueryString.HasValue)
        {
            logger.LogWarning("UploaderAccuroObservationsDropdowns: Invalid Review Date");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid Review Date", false));
        }
        var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
        if (!DateTime.TryParse(query["reviewDate"], out var reviewDate)) 
        {
            logger.LogWarning("UploaderAccuroObservationsDropdowns: Invalid Review Date");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid Review Date", false));
        }
        return await ExecuteSafeAsync(
               async () =>
               {
                   return await accuroObservationService.GetDropdownsByReviewDate(reviewDate);
               },
               $"Retrieved Reviewers and Labs for {query["reviewDate"]}"
           );

    }

    [Function("UploaderUpdateAccuroObservations")]
    public async Task<IActionResult> UpdateObservationGroupsUploads([HttpTrigger(AuthorizationLevel.Function, "patch", Route = "uploader/accuroObservations")]
        HttpRequest req)
    {
        var patchDTO = await req.ReadFromJsonAsync<AccuroObservationGroupPatchDTO>();
        if (patchDTO is null)
        {
            logger.LogWarning("UpdateObservationGroupsUploads: received empty payload");
            return new BadRequestObjectResult(new ApiResponse<string>("Invalid payload", false));
        }

        logger.LogInformation($"Patch Accuro Observation Groups");
        return await ExecuteCommandAsync(
               async () =>
               {
                   await accuroObservationService.UpdateObservationGroupsUploads(patchDTO);
               }, $"Accuro orders { string.Join(", ", patchDTO.Orders)} Updated"
           );

    }
}