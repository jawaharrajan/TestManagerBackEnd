using TestManager.Service.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;


namespace TestManager.Service.EventHubservices
{
public static class EventHubCDCMessageProcessor
    {
        private static readonly HttpClient _http = new();

        public static async Task<HttpResponseMessage> SendToExternalApiAsync(IConfiguration config, string payload)
        {
            string apiUrl = config["testclientAPIUrl"] ?? throw new ArgumentNullException("API_BASE_URL not set");
            string username = config["testclientAPIUrlUserId"] ?? throw new ArgumentNullException("UserId not set");
            string password = config["testclientAPIUrlPassword"] ?? throw new ArgumentNullException("Password not set");

            var handler = new HttpClientHandler
            {
                // Ignore SSL cert errors (e.g., self-signed, expired)
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using var client = new HttpClient(handler);

            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
           
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var headersDict = client.DefaultRequestHeaders.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
            DomainEventLogger.LogDomainEvent("SendingDataTotestclientApi", new Dictionary<string, object>
            {
                { "URL", apiUrl },
                { "Headers", headersDict},
                //{ "Retry", retryCount },
                //{ "Result", outcome.Exception},
                { "Payload", payload }
            });

            var response = await client.PostAsync(apiUrl, content);

            DomainEventLogger.LogDomainEvent("SendingDataTotestclientApi", new Dictionary<string, object>
            {
                { "URL", apiUrl },
                { "Headers", headersDict},
                //{ "Retry", retryCount },
                { "Result", response.StatusCode},
                { "Payload", payload }
            });

            return response;
        }
    }

}
