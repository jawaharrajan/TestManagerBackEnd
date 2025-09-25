using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TestManager.Service.EventHubservices
{
    public interface IProcessEventHubMessageService
    {
        Task<bool> ProcessEvents(EventData[] eventData);
    }

    public class ProcessEventHubMessageService : IProcessEventHubMessageService
    {
        private readonly IConfiguration _config;
        
        private readonly ILogger _logger;
        //private readonly ITableProcessor _tableProcessor;
        private readonly IEnumerable<ITableProcessor> _tableProcessors;

        public ProcessEventHubMessageService(IConfiguration config, 
            ILogger<ProcessEventHubMessageService> logger,
            IEnumerable<ITableProcessor> tableProcessors)
        {
            _config = config;
            _logger = logger;
            _tableProcessors = tableProcessors;
        }
        public async Task<bool> ProcessEvents(EventData[] events)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var grouped = new Dictionary<string, Dictionary<int, List<JsonElement>>>();
            string tableName = string.Empty;
            int op;
            string[] tablesToProcess = _config["testclientEventHubProcessTables"].Split(',') ?? throw new ArgumentNullException("Process Tables not set");            

            foreach (var eventData in events)
            {
                try
                {
                    var bytes = eventData.Data.ToArray();

                    var doc = JsonDocument.Parse(bytes);
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("TableName", out var tableNameProp)) continue;
                    if (!root.TryGetProperty("__$operation", out var opProp)) continue;

                    tableName = tableNameProp.GetString() ?? "Unknown";
                    op = opProp.GetInt32();

                    // need to skip all operation == 3
                    if (op == 3 || !tablesToProcess.Contains(tableName)) continue;

                    if (!grouped.ContainsKey(tableName))
                        grouped[tableName] = new Dictionary<int, List<JsonElement>>();

                    if (!grouped[tableName].ContainsKey(op))
                        grouped[tableName][op] = new List<JsonElement>();

                    grouped[tableName][op].Add(root);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize EventHub message.");
                }
            }

            foreach (var (tablName, opGroups) in grouped)
            {
                Console.WriteLine($"==> Table: {tablName}");

                foreach (var (operation, messages) in opGroups)
                {
                    Console.WriteLine($"   Operation {operation}: {messages.Count} messages");

                    var processor = _tableProcessors.FirstOrDefault(p => p.CanHandle(tablName.ToString()));
                    if (processor != null && tablesToProcess.Contains(tableName))
                    {
                        await processor.ProcessAsync(messages);
                    }
                    else
                    {
                        _logger.LogWarning("No processor found for TableName: {Table}", (tablName.ToString()));
                    }
                }
            }
            return true;
        }
    }
}
