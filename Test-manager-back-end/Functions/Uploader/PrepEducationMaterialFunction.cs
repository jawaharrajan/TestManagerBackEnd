using TestManager.Domain.DTO.Uploader;
using TestManager.Service.Helper;
using TestManager.Domain.Model.Uploader;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Uploader;

public class PrepEducationMaterialFunction(IPrepEducationMaterialService prepEducationMaterialService, ILogger<PrepEducationMaterialFunction> logger) : BaseFunction(logger)
{    
    [Function("UploaderGetPrepEducationMaterial")]
    public async Task<IActionResult> UploaderGetPrepEducationMaterial([HttpTrigger(AuthorizationLevel.Function, "get", Route ="uploader/educationmaterial")] HttpRequest req)
    {
        logger.LogInformation("Fetching all Uploader Education Materials");

        var result = await prepEducationMaterialService.GetEducationMaterial();

        logger.LogInformation($"Retrieved {result.Count} Education Materials");

        return new OkObjectResult(result);
    }

    [Function("UploaderGetPrepEducationMaterialById")]
    public async Task<IActionResult> UploaderGetPrepEducationMaterialById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/educationmaterial/{id}")] HttpRequest req, int? id)
    {
        logger.LogInformation("Fetching all Uploader Education Materials");

        if (!id.HasValue)
        {
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: Education Material Id is required.", false));

        }
        return await ExecuteSafeAsync(async () =>
        {
            var result = await prepEducationMaterialService.GetEducationMaterialById(id.Value) ??
                throw new KeyNotFoundException($"Education Material for Id: {id} Not found");
            return result;
        }, $"Get Education Material detail for {id}");
    }

    [Function("UploaderAddPrepEducationMaterial")]
    public async Task<IActionResult> UploaderAddPrepEducationMaterial([HttpTrigger(AuthorizationLevel.Function, "post", Route = "uploader/educationmaterial")] HttpRequest req)
    {
        logger.LogInformation("Adding new Uploader Prep Education Material");

        var prepEducationMaterialDTO = await req.ReadFromJsonAsync<PrepEducationMaterialDTO>();
        if (prepEducationMaterialDTO is null || string.IsNullOrEmpty(prepEducationMaterialDTO.Description))
        {
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: Prep Education Material cannot be null. Prep_EducationMaterial details missing", false));
        }

        return await ExecuteSafeAsync(
            async () =>
            {
                var id = await prepEducationMaterialService.AddEducationMaterial(prepEducationMaterialDTO);
                return id;
            }, "Adding Uploader Prep Education Material"
        );

    }

    [Function("UploaderUpdatePrepEducationMaterial")]
    public async Task<IActionResult> UploaderUpdatePrepEducationMaterial([HttpTrigger(AuthorizationLevel.Function, "put", Route = "uploader/educationmaterial")] HttpRequest req)
    {
        logger.LogInformation("Updating Uploader Prep Reporting Team");

        var prepEducationMaterialDTO = await req.ReadFromJsonAsync<PrepEducationMaterialDTO>();
        if (prepEducationMaterialDTO is null || prepEducationMaterialDTO.EducationMaterialId == 0 || string.IsNullOrEmpty(prepEducationMaterialDTO.Description))
        {
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: Prep Reporting Team cannot be null. PrepReportingTeam details missing", false));
        }

        return await ExecuteSafeAsync(
            async () =>
            {
                var id = await prepEducationMaterialService.UpdateEducationMaterial(prepEducationMaterialDTO)
                 ?? throw new KeyNotFoundException($"Prep Uploader Education Material with Id: {prepEducationMaterialDTO.EducationMaterialId} Not found");
                return id;
            }, "Updating Uploader Prep Education Material"
        );
    }

    [Function("UploaderDeletePrepEducationMaterial")]
    public async Task<IActionResult> UploaderDeletePrepEducationMaterial([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "uploader/educationmaterial/{id}")] HttpRequest req,
        int id)
    {
        logger.LogInformation("Deleting Uploader Prep Reporting Team");

        if (id <= 0)
        {
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: Prep Educatiom Material Id is required.", false));
        }

        return await ExecuteSafeAsync(
            async () =>
            {
                var deleted = await prepEducationMaterialService.DeleteEducationMaterial(id);
                if (!deleted)
                {
                    throw new KeyNotFoundException($"Prep EducationMaterial ID {id} not found.");
                }
                // Fetch updated list after successful delete
                var remaining = await prepEducationMaterialService.GetEducationMaterial();
                return remaining;
            }, $"Deleting Uploader Education Material with Id: {id} successfully"
        );
    }
}