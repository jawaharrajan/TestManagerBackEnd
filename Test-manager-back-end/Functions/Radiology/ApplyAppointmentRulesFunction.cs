using System;
using System.Net;
using TestManagerBackEnd.Functions.Radiology;
using TestManager.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestManagerBackEnd.Functions.Radiology;

public class ApplyAppointmentRulesFunction(IApplyAppointmentRulesService applyAppointmentRulesService, ILogger<AppointmentRulesFunction> logger)
{

    [Function("RunAppointmentRules")]
    public async Task RunAppointmentRulesAsync([TimerTrigger("%AppointmentRulesSchedule%")] TimerInfo myTimer)
    {
        logger.LogInformation("Appointment Rules Timer trigger at executed at: {executionTime}", DateTime.Now);
        var now = DateTime.UtcNow;
        var last = myTimer.ScheduleStatus?.Last ?? DateTime.MinValue;
        var next = myTimer.ScheduleStatus?.Next ?? DateTime.MinValue;

        try
        {

            logger.LogInformation($"Inside Function at : {DateTime.Now} ");
            // Guard clause: prevent duplicate/early runs caused by skew or early execution
            if (now < next)
            {
                logger.LogWarning("Duplicate or early execution skipped. Now: {now}, Next: {next}", now, next);
                return;
            }

            var result = await applyAppointmentRulesService.RunAppointmentRules();
            logger.LogInformation($"Appointment Rules ran {(result ? "Successfully" : "Unsuccessfully")} ");

            if (myTimer.ScheduleStatus is not null)
            {                
                logger.LogInformation("Next schedule: {nextSchedule}", myTimer.ScheduleStatus.Next);                
            }           
        }
        catch (Exception ex)
        {
            logger.LogError($"Timer error: {ex.Message}");
        }

    }
}