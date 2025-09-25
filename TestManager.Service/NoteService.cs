using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Service.Helper;
using TestManager.Service.Logging;
using TestManager.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TestManager.Service
{
    public interface INoteService
    {
        Task<bool> AddAppointmentNotesAsync(NoteDTO noteDTO);        
        Task<List<NoteDTO>> GetNotes(int entityTypeId, int instanceId);     

    }
    public class NoteService(ILogger<NoteService> logger, IConfiguration config, 
            IHttpClientFactory httpClientFactory, 
            INoteRepository noteRepository,
            //IUserRepository userRepository,
            IUserContextService userContextService) : INoteService
    {
        public async Task<bool> AddAppointmentNotesAsync(NoteDTO noteDTO)
        {
            //create new Payload for testclient API
            var payload = new NotePayload
            {
                actionid = 13,
                entityid = null,
                entitytype = "Note",
                data = noteDTO.Text,
                subentityid = noteDTO.InstanceID,
                subentitytype = "Appointment",  
                userid = userContextService.TipsUserId,
                _comment = $"Updating notes in the Note table by: {userContextService.TipsUserId}"
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            var response = await SendToExternalApi.SendAsync(config, httpClientFactory, jsonPayload);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<NoteDTO>> GetNotes(int entityTypeId, int instanceId)
        {
            return await noteRepository.GetNotes(entityTypeId, instanceId);
        }

        //public async Task<bool> SendToExternalApiAsync(string payload)
        //{
        //    string apiUrl = config["testclientAPIUrl"] ?? throw new ArgumentNullException("API_BASE_URL not set");
        //    string username = config["testclientAPIUrlUserId"] ?? throw new ArgumentNullException("UserId not set");
        //    string password = config["testclientAPIUrlPassword"] ?? throw new ArgumentNullException("Password not set");

        //    using var client = httpClientFactory.CreateClient("testclientConditionalSSLCertBypass");
        //    //using var client = new HttpClient();
            
        //    var creds = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);

        //    var content = new StringContent(payload, Encoding.UTF8, "application/json");

        //    var retryPolicy = Policy
        //      .Handle<HttpRequestException>()
        //      .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        //      .WaitAndRetryAsync(
        //        retryCount: 3,
        //        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),  // 2s, 4s, 8s
        //        onRetry: (outcome, timespan, retryCount, context) =>
        //        {
        //            var headersDict = client.DefaultRequestHeaders.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
        //            if (outcome.Exception != null)
        //            {
        //                logger.LogWarning(outcome.Exception, "Retry {Retry} after exception, waiting {Delay}s", retryCount, timespan.TotalSeconds);

        //                DomainEventLogger.LogDomainEvent("SendNoteTotestclientApi", new Dictionary<string, object>
        //                {
        //                    { "URL", apiUrl },
        //                    { "Headers", headersDict},
        //                    { "Retry", retryCount },
        //                    { "Result", outcome.Exception},
        //                    { "Payload", payload }
        //                });
        //            }
        //            else
        //            {
        //                logger.LogWarning("Retry {Retry} after HTTP {StatusCode}, waiting {Delay}s", retryCount, outcome.Result.StatusCode, timespan.TotalSeconds);
        //                DomainEventLogger.LogDomainEvent("SendNoteTotestclientApi", new Dictionary<string, object>
        //                {
        //                    { "URL", apiUrl },
        //                    { "Headers", headersDict},
        //                    { "Retry", retryCount },
        //                    { "Result", outcome.Result},
        //                    { "Payload", payload }
        //                });
        //            }

        //        });

        //    // Execute the HTTP call under the policy
        //    var response = await retryPolicy.ExecuteAsync(() => client.PostAsync(apiUrl, content));
        //    return response.IsSuccessStatusCode;

        //    #region -Manual retry
        //    //const int maxAttempts = 3;
        //    //for (int attempt = 1; attempt <= maxAttempts; attempt++)
        //    //{
        //    //    var headersDict = client.DefaultRequestHeaders.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
        //    //    try
        //    //    {
        //    //        var response = await client.PostAsync(apiUrl, content);

        //    //        if (response.IsSuccessStatusCode)
        //    //            return true;

        //    //        logger.LogWarning($"Attempt: {attempt} failed: {response.StatusCode}");
        //    //        DomainEventLogger.LogDomainEvent("SendNoteTotestclientApi", new Dictionary<string, object>
        //    //                    {
        //    //                        { "URL", apiUrl },
        //    //                        { "Headers", headersDict},
        //    //                        { "Retry", attempt },
        //    //                        { "Result",response.StatusCode},
        //    //                        { "Payload", payload }
        //    //                    });
        //    //    }
        //    //    catch (Exception ex) when (attempt < maxAttempts)
        //    //    {
        //    //        logger.LogWarning(ex, "Attempt {Attempt} threw, retrying...", attempt);
        //    //        DomainEventLogger.LogDomainEvent("SendNoteTotestclientApi", new Dictionary<string, object>
        //    //                    {
        //    //                        { "URL", apiUrl },
        //    //                        { "Headers", headersDict},
        //    //                        { "Retry", attempt },
        //    //                        { "Result", ex.Message},
        //    //                        { "Payload", payload }
        //    //                    });
        //    //    }
        //    //    // simple linear back-off (delay = 1s, 2s, 3s)
        //    //    await Task.Delay(TimeSpan.FromSeconds(attempt));
        //    //}

        //    //logger.LogError($"All {maxAttempts} attempts to send to {apiUrl} failed");
        //    //return false;
        //    #endregion
        //}
    }
}
