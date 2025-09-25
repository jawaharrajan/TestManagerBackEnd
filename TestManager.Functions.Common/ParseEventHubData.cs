using Azure.Messaging.EventHubs;
using TestManager.Domain.Model.EventHubModels;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace TestManager.Service.Helper
{
    public static class ParseEventHubData
    {
        public static CdcEntityStatus? ParseEventData(EventData eventData, ILogger logger)
        {
            try
            {
                var json = Encoding.UTF8.GetString(eventData.Body.ToArray());
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<CdcEntityStatus>(json, options);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Deserialization error in ParseEventData");
                return null;
            }
        }
    }
}
