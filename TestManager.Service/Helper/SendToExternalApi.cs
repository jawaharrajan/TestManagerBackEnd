using TestManager.Service.Logging;
using Microsoft.Extensions.Configuration;
using Polly;
using System.Net.Http.Headers;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Net;
using System.Net.Http;

namespace TestManager.Service.Helper
{
    public static class SendToExternalApi
    {        
        public static async Task<HttpResponseMessage?> SendAsync(IConfiguration config, IHttpClientFactory httpClientFactory, string payload)
        {
            string username = string.Empty; string password = string.Empty; string apiUrl = string.Empty;

            try
            {
                apiUrl = config["testclientAPIUrl"] ?? throw new ArgumentNullException("API_BASE_URL not set");

                var keyVaultUrl = config["AZURE_KEY_VAULT_URL"];
                var keyVaultTipsAPIClientId = config["testclientAPIUrlKVClientId"];
                var keyVaultTipsAPIClientSecret = config["testclientAPIUrlKVClientSecret"];

                if (config["APP_Environment"] == "local")
                {
                    username = keyVaultTipsAPIClientId;
                    password = keyVaultTipsAPIClientSecret;
                }
                else
                {                    
                    var kvClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
                    KeyVaultSecret clientIdSecret = await kvClient.GetSecretAsync(keyVaultTipsAPIClientId);
                    KeyVaultSecret passwordSecret = await kvClient.GetSecretAsync(keyVaultTipsAPIClientSecret);                    
                    
                    username = clientIdSecret.Value;
                    password = passwordSecret.Value;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
                DomainEventLogger.LogDomainEvent("SendTotestclientApiKVError", new Dictionary<string, object>
                {
                    {"action", "POST" },
                    {"Error", ex.Message }
                });
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Key vault not valid"
                };
            }
            
            using var client = httpClientFactory.CreateClient("testclientConditionalSSLCertBypass");           

            var creds = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var retryPolicy = Policy
              .Handle<HttpRequestException>()
              .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
              .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),  // 2s, 4s, 8s
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    var headersDict = client.DefaultRequestHeaders.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
                    var logEntry = new Dictionary<string, object>
                    {
                        { "URL", apiUrl },
                        { "Headers", headersDict },
                        { "Retry", retryCount },
                        { "Payload", payload }
                    };

                    if (outcome.Exception != null)
                    {                        
                        logEntry["Result"] = outcome.Exception;
                    }
                    else
                    {
                        logEntry["Result"] = outcome.Result;
                    }
                    DomainEventLogger.LogDomainEvent("SendTotestclientApi", logEntry);
                });

            // Execute the HTTP call under the policy
            var response = await retryPolicy.ExecuteAsync(async () =>
            {
                var result = await client.PostAsync(apiUrl, content);

                var headersDict = client.DefaultRequestHeaders.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
                var logEntry = new Dictionary<string, object>
                {
                    { "URL", apiUrl },
                    { "Headers", headersDict },
                    { "Retry", 0 }, // First attempt
                    { "Payload", payload },
                    { "Result", result }
                };

                DomainEventLogger.LogDomainEvent("SendTotestclientApi-Attempt", logEntry);

                return result;
            });

            return response;                     
        }
    }
}
