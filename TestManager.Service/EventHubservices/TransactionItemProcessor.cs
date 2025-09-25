using TestManager.DataAccess.Repository.Contracts;
using TestManager.Service.Helper;
using BackendModels = TestManager.Domain.Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Transactions;
using TestManager.Domain.Model;
using TestManager.Service.Logging;
using TestManager.Domain.DTO.EventHub;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace TestManager.Service.EventHubservices
{
    public class TransactionItemProcessor(ILogger<TransactionItemProcessor> logger, 
        IGenericRepository<BackendModels.Transaction, int> genericRepository,
        ICMS_SyncTrackingRepository syncRepository,
        IConfiguration config,
         IHttpClientFactory httpClientFactory) : ITableProcessor
    {
        public bool CanHandle(string tableName) => tableName.Equals("TransactionItem", StringComparison.OrdinalIgnoreCase);

        public async Task ProcessAsync(List<JsonElement> jsonElements)
        {

            Console.WriteLine($"[Processor] Received {jsonElements.Count} TransactionItem records.");

            await ProcessRecordsAsync(jsonElements);
        }

        public async Task ProcessRecordsAsync(List<JsonElement> jsonElements)
        {
            var records = jsonElements
                .Select(e => e.ToDictionary())
                .Where(d =>
                    d.TryGetValue("TransactionId", out var _) &&
                    d.TryGetValue("__$operation", out var op) &&
                    d.TryGetValue("TransactionItemId", out var transItemId) &&  // Adding TransactionItemId check
                    //d.TryGetValue("AccessionNo", out var accNo) &&
                    op != null && Convert.ToInt32(op) != 3 && // Skip op = 3 &&
                    //accNo != null && Convert.ToInt32(accNo) != -1 &&
                    transItemId != null && Convert.ToInt32(transItemId) != -1
                ).ToList();


            // Handle Inserts & Deletes (op 1 & 2)
            var insertOrDeleteRecords = records
                .Where(d =>
                    d.TryGetValue("__$operation", out var op) &&
                    d.TryGetValue("TransactionId", out var txn) &&
                    d.TryGetValue("TransactionItemId", out var transItemId) &&
                    //d.TryGetValue("AccessionNo", out var accNo) &&
                    Convert.ToInt32(d["__$operation"]) is 1 or 2
                ).ToList();

            foreach (var record in insertOrDeleteRecords)
            { 
                var operation = Convert.ToInt32(record["__$operation"]);
                var transactionId = Convert.ToInt32(record["TransactionId"]);
                int transItemId = Convert.ToInt32(record["TransactionItemId"]);
                //int accessionNo = Convert.ToInt32(record["AccessionNo"]);

                try
                {
                    var appointment = await genericRepository.GetProjectedByIdAsync(
                        e => e.TransactionId == transactionId,
                        e => new { e.InstanceId });

                    var tipsId = await syncRepository.GetTIPSIdNoFlag("TransactionItem", transItemId);

                    var payload = new testclientDataPayload
                    {                    
                        actionid = operation == 1 ? 2 : 1,
                        entityid = Convert.ToInt32(appointment.InstanceId),
                        entitytype = "appointment",
                        data = string.Empty,
                        subentityid = tipsId,
                        subentitytype = "product",
                        _comment = operation == 1 ? "Deleting products from an appointment" : "Adding products to an Appointment"
                    };

                    string jsonPayload = JsonSerializer.Serialize(payload);
                    var response = await SendToExternalApi.SendAsync(config, httpClientFactory, jsonPayload);
                    Console.WriteLine($"POST {response.StatusCode}: {JsonSerializer.Serialize(payload)}");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error: {ex.Message}");
                    continue;
                }
            }

            // Handle Update (op 4)
            var updateGroups = records
                .Where(d => Convert.ToInt32(d["__$operation"]) == 4)
                .GroupBy(d => Convert.ToInt32(d["TransactionId"]));

            foreach (var g in updateGroups)
            {
                //Get the TransactionItemId .
                var transactionItemIds = g
                    .Where(d => d.ContainsKey("TransactionItemId") && d["TransactionItemId"] != null)
                    .Select(d => d["TransactionItemId"]!.ToString())
                    .Distinct().OrderBy(id => id).ToList();

                var tipsIdList = new List<int>();

                foreach (var item in transactionItemIds)
                {
                    int id = await syncRepository.GetTIPSIdNoFlag("TransactionItem",Int32.Parse(item));
                    tipsIdList.Add(id);
                }

                tipsIdList.Sort();

                string realTipsIds = string.Join(",", tipsIdList); // Comma-separated

                //Get the current joined AccessionNumber
                var accessionNo = g
                    .Where(d => d.ContainsKey("AccessionNo") && d["AccessionNo"] != null)
                    .Select(d => d["AccessionNo"]!.ToString())
                    .Distinct().FirstOrDefault(); // always one only 

                var joinedAccessionIds = accessionNo + "," + string.Join(",", transactionItemIds);

                try
                {
                    //var appointment = await genericRepository.GetByIdAsync(g.Key);
                    var appointment = await genericRepository.GetProjectedByIdAsync(
                        e => e.TransactionId == g.Key,
                        e => new { e.InstanceId });

                    var payload = new testclientProductJoinDataPayload
                    {
                        actionid = 3,
                        entityid = Convert.ToInt32(appointment.InstanceId),
                        entitytype = "appointment",
                        data = string.Empty,
                        subentityid = realTipsIds,
                        subentitytype = "product",                   
                        _comment = "Joining multiple products of an appointment into 1 accession number"
                    };

                    string jsonPayload = JsonSerializer.Serialize(payload);
                    var response = await SendToExternalApi.SendAsync(config, httpClientFactory, jsonPayload);
                    Console.WriteLine($"POST {response.StatusCode}: {JsonSerializer.Serialize(payload)}");
                }
                catch(Exception ex)
                {
                    logger.LogError($"Error: {ex.Message}");
                    continue;
                }
            }
        }
    }
}
