using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Service.Helper;
using TestManager.Domain.Model;
using TestManager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Radiology
{
    public class AppointmentRulesFunction(IAppointmentRuleService appointmentService, ILogger<AppointmentRulesFunction> logger) : BaseFunction(logger)
    {
        [Function("GetAppointmentRules")]
        public async  Task<IActionResult> GetAppointmentRules([HttpTrigger(AuthorizationLevel.Function, "get", Route ="appointmentrules")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Fetching all Appointment Rules");

            var rules = await appointmentService.GetAllAppointmentRules();

            logger.LogInformation($"Retrieved {rules.Count()} Appointment Rules");

            return new OkObjectResult(rules);
        }

        [Function("AddAppointmentRule")]
        public async Task<IActionResult> AddAppointmentRule([HttpTrigger(AuthorizationLevel.Function, "post", Route = "appointmentrules")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Adding new Appointment rule");

            var appointmentRule = await req.ReadFromJsonAsync<AppointmentRuleDTO>();

            if (appointmentRule is null)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Appointment Rule cannot be null.", false));
            }

            return await ExecuteSafeAsync(
            async () =>
            {
                var newRecord = await appointmentService.AddAppointmentRule(appointmentRule);
                return newRecord;
            }, "Appointment Rule added successfully");
        }

        [Function("UpdateAppointmentRule")]
        public async Task<IActionResult> UpdateAppointmentRule([HttpTrigger(AuthorizationLevel.Function, "put", Route = "appointmentrules")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            var appointmentRule = await req.ReadFromJsonAsync<AppointmentRuleDTO>();

            if (appointmentRule is null)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload:  Appointment Rule cannot be null. AppointmentRule details missing", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var result = await appointmentService.UpdateAppointmentRule(appointmentRule)
                        ?? throw new KeyNotFoundException($"Andrologist with Id: {appointmentRule.Id} Not found");
                    return result;
                }, "AppointmentRule updated successfully");
        }

        [Function("DeleteAppointmentRule")]
        public async Task<IActionResult> DeleteAppointmentRule([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "appointmentrules/{Id}")] HttpRequest req,
                    int id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            if (id <= 0)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: AppointmentRule Id is required.", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var deleted = await appointmentService.DeleteAppointmentRule(id);
                    if (!deleted)
                    {
                        throw new KeyNotFoundException($"Appointment Rule with ID {id} not found.");
                    }
                    // Fetch updated list after successful delete
                    var remaining = await appointmentService.GetAllAppointmentRules();
                    return remaining;
                }, $"Deleted AppointmentRule with Id: {id} successfully");
        }
    }
}
