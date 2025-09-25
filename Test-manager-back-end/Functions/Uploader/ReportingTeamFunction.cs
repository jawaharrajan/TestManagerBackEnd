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
    public class ReportingTeamFunction(IPrepReportingTeamService reportingTeamService, ILogger<ReportingTeamFunction> logger) : BaseFunction(logger)
    {      

        [Function("UploaderGetReportingTeamAndTempaltes")]
        public async Task<IActionResult> UploaderGetReportingTeamAndTempaltes([HttpTrigger(AuthorizationLevel.Function, "get" , Route= "uploader/reportingTeamTemplates")] HttpRequest req)
        {
            logger.LogInformation("Fetching all Reporting Team and Templates");

            var result = await reportingTeamService.GetReportingTeamWithTemplateAndTemplateText();

            logger.LogInformation($"Retrieved {result.CountBy(r => r.PrepReportingTeamTemplate.Count)} Reporting team data");

            return new OkObjectResult(result);
        }

        [Function("UploaderGetReportingTeam")]
        public async Task<OkObjectResult> UploaderGetReportingTeam([HttpTrigger(AuthorizationLevel.Function, "get", Route = "uploader/reportingteam")] HttpRequest req)
        {
            logger.LogInformation("Fetching all Uploader Prep Reporting Teams");

            var reportingTeamDTO = await reportingTeamService.GetReportingTeam();

            logger.LogInformation($"Retrieved {reportingTeamDTO.Count} Prep Reporting Teams");

            return new OkObjectResult(reportingTeamDTO);
        }

        [Function("UploaderAddReportingTeam")]
        public async Task<IActionResult> UploaderAddReportingTeam([HttpTrigger(AuthorizationLevel.Function, "post", Route = "uploader/reportingteam")] HttpRequest req)
        {
            logger.LogInformation("Adding new Uploader Prep Reporting Team");

            var reportingTeamDTO = await req.ReadFromJsonAsync<ReportingTeamDTO>();
            if (reportingTeamDTO is null || string.IsNullOrEmpty(reportingTeamDTO.ReportingTeamName))
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Prep Reporting Team cannot be null. PrepReportingTeam details missing", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var id = await reportingTeamService.AddReportingTeam(reportingTeamDTO);
                    return id;
                }, "Adding Uploader Reporting Team"
            );
        }

        [Function("UploaderUpdateReportingTeam")]
        public async Task<IActionResult> UploadeeUpdateReportingTeam([HttpTrigger(AuthorizationLevel.Function, "put", Route = "uploader/reportingteam")] HttpRequest req)
        {
            logger.LogInformation("Updating Uploader Prep Reporting Team");

            var reportingTeamDTO = await req.ReadFromJsonAsync<ReportingTeamDTO>();
            if (reportingTeamDTO is null || string.IsNullOrEmpty(reportingTeamDTO.ReportingTeamName))
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Prep Reporting Team cannot be null. PrepReportingTeam details missing", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var id = await reportingTeamService.UpdateReportingTeam(reportingTeamDTO)
                     ?? throw new KeyNotFoundException($"Prep Reporting Team with Id: {reportingTeamDTO.ReportingTeamId} Not found");
                    return id;
                }, "Updating Uploader Prep Reporting Team"
            );
        }

        [Function("UploaderDeleteReportingTeam")]
        public async Task<IActionResult> UploaderDeleteReportingTeam([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "uploader/reportingteam/{id}")] HttpRequest req,
            int id)
        {
            logger.LogInformation("Deleting Uploader Prep Reporting Team");
            
            if (id <= 0)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Prep Reporting Team Id is required.", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var deleted = await reportingTeamService.DeleteReportingTeam(id);
                    if (!deleted)
                    {
                        throw new KeyNotFoundException($"Prep Reporting Team ID {id} not found.");
                    }
                    // Fetch updated list after successful delete
                    var remaining = await reportingTeamService.GetReportingTeam();
                    return remaining;
                }, $"Deleting Uploader Prep Reporting Team with Id: {id} successfully"
            );
        }
    }
}
