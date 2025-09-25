using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TestManager.Service.EventHubservices
{
    public class DicommModalityProcessor(ILogger<DicommModalityProcessor> logger) : ITableProcessor
    {
        public bool CanHandle(string tableName) => tableName.Equals("DICOMModality", StringComparison.OrdinalIgnoreCase);

        public async Task ProcessAsync(List<JsonElement> jsonElements)
        {
            //var entityId = Convert.ToInt32(data["DICOMModalityId"]);
            //logger.LogInformation($"Processing DICOMModality with ID {entityId}");
            logger.LogInformation($"No Processing DICOMModality at present");
            // handle domain logic
            // TODO: Add logic to handle Entity Status when the API at testclient is ready to handle additional Entiity Types
            await Task.CompletedTask;
        }
    }
}
