using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;
using TestManagerBackEnd.Functions.Radiology;
using TestManager.Service.Helper;
using TestManager.Domain.Model;
using TestManager.Service;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Uploader;

public class PrepTemplateFunction(IPrepTemplateService prepTemplateService, ILogger<PrepTemplateFunction> logger) : BaseFunction(logger)
{

    [Function("GetPrepTemplate")]
    public async Task<OkObjectResult> GetPrepTemplate([HttpTrigger(AuthorizationLevel.Function, "get", Route ="uploader/template")] HttpRequest req)
    {
        logger.LogInformation("Fetching all Uploader Prep Templates");       

        var prepTemplates = await prepTemplateService.GetPrepTemplatesAysnc();

        logger.LogInformation($"Retrieved {prepTemplates.Count} Prep Templates");

        return new OkObjectResult(prepTemplates);
    }

    [Function("AddPrepTemplate")]
    public async Task<IActionResult> AddPrepTemplate([HttpTrigger(AuthorizationLevel.Function, "post", Route = "uploader/template")] HttpRequest req)
    {
        logger.LogInformation("Adding new Uploader Prep Template");

        var prepTemplateDTO = await req.ReadFromJsonAsync<PrepTemplateDTO>();
        if (prepTemplateDTO is null || string.IsNullOrEmpty(prepTemplateDTO.Text) || string.IsNullOrEmpty(prepTemplateDTO.Description))
        {
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: Prep Template cannot be null. PrepTemplate details missing", false));
        }

        return await ExecuteSafeAsync(
            async () =>
            {
                var id = await prepTemplateService.AddPrepTemplateAsync(prepTemplateDTO);
                return id;
            }, "Adding Uploader Prep Template"
        );
    }

    [Function("UpdatePrepTemplate")]
    public async Task<IActionResult>UpdatePrepTemplate([HttpTrigger(AuthorizationLevel.Function, "put", Route = "uploader/template")] HttpRequest req)
    {
        logger.LogInformation("Updating Uploader Prep Template");

        var prepTemplateDTO = await req.ReadFromJsonAsync<PrepTemplateDTO>();
        if (prepTemplateDTO is null || string.IsNullOrEmpty(prepTemplateDTO.Text) || string.IsNullOrEmpty(prepTemplateDTO.Description))
        {
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: Prep Template cannot be null. PrepTemplate details missing", false));
        }

        return await ExecuteSafeAsync(
            async () =>
            {
                var id = await prepTemplateService.UpdatePrepTemplateAsync(prepTemplateDTO)
                 ?? throw new KeyNotFoundException($"PrepTemplate with Id: {prepTemplateDTO.TemplateId} Not found");
                return id;
            }, "Updating Uploader Prep Template"
        );
    }

    [Function("DeletePrepTemplate")]
    public async Task<IActionResult> DeletePrepTemplate([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "uploader/template/{id}")] HttpRequest req,
        int id)
    {
        logger.LogInformation("Deleting Uploader Prep Template");

        // EnrichLoggingFromRequest(req, enricher);
        if (id <= 0)
        {
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: PrepTempalte Id is required.", false));
        }

        return await ExecuteSafeAsync(
            async () =>
            {
                var deleted = await prepTemplateService.DeletePrepTemplateAsync(id);
                if (!deleted)
                {
                    throw new KeyNotFoundException($"PrepTemplate with TemplateID {id} not found.");
                }
                // Fetch updated list after successful delete
                var remaining = await prepTemplateService.GetPrepTemplatesAysnc();
                return remaining;
            }, $"Deleted Uploader Prep Template with Id: {id} successfully"
        );
    }
}