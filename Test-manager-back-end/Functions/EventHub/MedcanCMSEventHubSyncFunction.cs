using System;
using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using TestManager.Domain.Model.EventHubModels;
using TestManager.Service.EventHubservices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TestManagerBackEnd.Functions.EventHub;
public class testclientCMSEventHubSyncFunction(IProcessEventHubMessageService processEventhubMessageService , ILogger<testclientCMSEventHubSyncFunction> logger)
{

    [Function(nameof(testclientCMSPostTipsSyncFunction))]
    public async Task testclientCMSPostTipsSyncFunction([EventHubTrigger("%testclient_CMS_EVENT_HUB_NAME%", Connection = "EVENT_HUB_CONNECTIONSTRING")] EventData[] events)
    {
        logger.LogInformation($"Invoked with {events.Length} event(s)");

        await processEventhubMessageService.ProcessEvents(events);
    }
}