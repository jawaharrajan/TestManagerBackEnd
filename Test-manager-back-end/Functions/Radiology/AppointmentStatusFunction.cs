using TestManager.Domain.DTO;
using TestManager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Radiology
{
    public class AppointmentStatusFunction(IAppointmentStatusService appointmentStatusService, ILogger<AppointmentStatusFunction> logger) : BaseFunction(logger)
    {
        [Function("GetAppointmentStatus")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route ="AppointmentStatus")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Fetching all Radiology Appointment Statues");

            return await ExecutePagedAsync<AppointmentStatusDTO>(
            async () =>
            {
                var statues = await appointmentStatusService.GetAppointmentStatusAsync() ??
                    throw new KeyNotFoundException($"No Radiology AppointmentSatuses found");
                return (statues, statues.Count);
            }, 1, 0, "Get Radiology Appointment statues");
        }
    }
}
