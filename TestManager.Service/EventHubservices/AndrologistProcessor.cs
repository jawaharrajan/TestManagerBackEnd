using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TestManager.Service.EventHubservices
{
   public class AndrologistProcessor(ILogger<AndrologistProcessor> logger,
       IConfiguration config) : ITableProcessor
    {
        public bool CanHandle(string tableName) => tableName.Equals("Andrologist", StringComparison.OrdinalIgnoreCase);

        public async Task ProcessAsync(List<JsonElement> jsonElements)
        {
            //var entityId = Convert.ToInt32(data["AndrologistID"]);
            //logger.LogInformation($"Processing Andrologist with ID {entityId}");
            logger.LogInformation($"No Processing Andrologist at present");
            // handle domain logic
            // TODO: Add logic to handle Entity Status when the API at testclient is ready to handle additional Entity Types


            await Task.CompletedTask;
        }
    }
}
