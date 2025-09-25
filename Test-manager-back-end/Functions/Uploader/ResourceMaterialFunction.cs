using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestManagerBackEnd.Functions.Uploader;

public class ResourceMaterialFunction(IPrepResourceCategoryService prepResourceCategoryService, ILogger<ResourceMaterialFunction> logger)
{
    [Function("UploaderResourceEducationMaterialbyResourceId")]
    public async Task<IActionResult> UploaderResourceEducationMaterialbyResourceId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/getResourceEducationMaterial/{id}")] 
        HttpRequest req, int? id)
    {
        logger.LogInformation($"Fetching all Education Materials for ResourceId {id.Value}");

        var result = await prepResourceCategoryService.GetResourceWithEducationMaterialByResourceId(id.Value);

        logger.LogInformation($"Retrieved {result.CountBy(r => r.EducationMaterials.Count)} Materials for Resource: {id.Value}");

        return new OkObjectResult(result);
    }

    [Function("UploaderGetResourceEducationMaterials")]
    public async Task<IActionResult> UploaderGetResourceEducationMaterials([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/getResourceEducationMaterial")]
        HttpRequest req)
    {
        logger.LogInformation($"Fetching all Resource Education Materials");

        var result = await prepResourceCategoryService.GetResourceWithEducationMaterials();

        logger.LogInformation($"Retrieved {result.CountBy(r => r.EducationMaterials.Count)} Materials for Resources");

        return new OkObjectResult(result);
    }
}