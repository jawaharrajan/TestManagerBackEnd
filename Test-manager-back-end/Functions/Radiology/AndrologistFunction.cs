using TestManager.DataAccess.Repository;
using TestManager.Domain.DTO;
using TestManager.Service.Helper;
using TestManager.Domain.Model;
using TestManager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Radiology
{
    public class AndrologistFunction(IAndrologistService andrologistService, ILogger<AndrologistFunction> logger) : BaseFunction(logger)
    {
        [Function("GetAndrologist")]
        public async Task<OkObjectResult> GetAndrologist([HttpTrigger(AuthorizationLevel.Function, "get", Route = "andrologists")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Fetching all Andrologists");

            var filter = new AndrologistFilterDTO();
            if (req.QueryString.HasValue)
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
                filter = new AndrologistFilterDTO
                {                    
                    SearchTerm = query["searchTerm"],                    
                };
            }

            var andrologists = await andrologistService.GetAndrologistsAsync(filter);

            logger.LogInformation($"Retrieved {andrologists.Count} Andrologists");

            return new OkObjectResult(andrologists);
        }

        [Function("AddAndrologist")]
        public async Task<IActionResult> AddAndrologist([HttpTrigger(AuthorizationLevel.Function, "post", Route = "andrologists")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Adding new Andrologist");

            var andrologist = await req.ReadFromJsonAsync<AndrologistDto>();

            if (andrologist is null || string.IsNullOrEmpty(andrologist.LastName) || string.IsNullOrEmpty(andrologist.FirstName))
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Andrologist cannot be null. Adnrologist details missing", false));
            }
                
            return await ExecuteSafeAsync(
            async () =>
                {
                    var id = await andrologistService.AddAndrologist(andrologist);
                    return id;
                },
                "Andrologist added successfully"
            );
        }

        [Function("UpdateAndrologist")]
        public async Task<IActionResult> UpdateAndrologist([HttpTrigger(AuthorizationLevel.Function, "put", Route = "andrologists")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            var andrologist = await req.ReadFromJsonAsync<AndrologistDto>();

            if (andrologist is null || string.IsNullOrEmpty(andrologist.LastName) || string.IsNullOrEmpty(andrologist.FirstName))
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Andrologist cannot be null. Adnrologist details missing", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var result = await andrologistService.UpdateAndrologist(andrologist)
                        ?? throw new KeyNotFoundException($"Andrologist with Id: {andrologist.AndrologistId} Not found");
                    return result;
                },
                "Andrologist updated successfully"
            );
        }

        [Function("DeleteAdnrologist")]
        public async Task<IActionResult> DeleteAndrologist([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "andrologists/{Id}")] HttpRequest req,
                    int id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            if (id <= 0)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: AndrologistId is required.", false));
            }
           
            return await ExecuteSafeAsync(
                async () =>
                {
                    var deleted = await andrologistService.DeleteAdnrologist(id);
                    if (!deleted)
                    {
                        throw new KeyNotFoundException($"Andrologist with ID {id} not found.");
                    }
                    // Fetch updated list after successful delete
                    var remaining = await andrologistService.GetAndrologistsAsync();
                    return remaining;
                },
                $"Deleted Andrologist with Id: {id} successfully"
            );
        }
    }   
}
