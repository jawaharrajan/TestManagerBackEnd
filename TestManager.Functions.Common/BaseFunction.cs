using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using TestManager.Service.Helper;
using TestManager.Service.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TestManager.Functions.Common
{
    public abstract class BaseFunction(ILogger logger, IConfiguration config = null)
    {
        protected async Task<IActionResult> ExecuteSafeAsync<T>(
            Func<Task<T>> action,
            string actionName)
        {
            try
            {
                var result = await action();
                return new OkObjectResult(new ApiResponse<T>(result, actionName));
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, $"{actionName} - Not found.");
                return new NotFoundObjectResult(new ApiResponse<string>(ex.Message, false));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{actionName} - Error");
                return new ObjectResult(new ApiResponse<string>("An unexpected error occurred.", false))
                {
                    StatusCode = 500
                };
            }
        }

        protected async Task<IActionResult> ExecutePagedAsync<T>(
            Func<Task<(IEnumerable<T> Data, int Total)>> action,
            int page,
            int pageSize,
            string actionName)
        {
            try
            {
                var (data, total) = await action();
                return new OkObjectResult(new PagedApiResponse<T>(data, total, page, pageSize, actionName));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{actionName} - Error");
                return new ObjectResult(new PagedApiResponse<T>("An unexpected error occurred."))
                {
                    StatusCode = 500
                };
            }
        }

        protected async Task<IActionResult> ExecuteCommandAsync(
            Func<Task> action,
            string actionName,
            string? successMessage = null)
        {
            try
            {
                await action();
                return new OkObjectResult(new ApiResponse<string>(successMessage ?? "Success", true));
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, $"{actionName} - Not found.");
                return new NotFoundObjectResult(new ApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{actionName} - Error");
                return new ObjectResult(new ApiResponse<string>("An unexpected error occurred.", false))
                {
                    StatusCode = 500
                };
            }
        }

        protected async Task<Dictionary<string, string>> GetKeyVaultSecrets(IEnumerable<string> keys) 
        {
            try
            {
                Dictionary<string, string> dict = [];
                var keyVaultUrl = config["AZURE_KEY_VAULT_URL"];
                var kvClient = keyVaultUrl != null && config["APP_Environment"] != "local"
                    ? new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential())
                    : null;
                foreach (var key in keys)
                {
                    if (!dict.ContainsKey(key))
                    {
                        string? value = null;
                        if (kvClient == null)
                        {
                            value = config[key];
                        }
                        else 
                        {
                            KeyVaultSecret secret = await kvClient.GetSecretAsync(config[key]);
                            value = secret.Value;
                        }
                        dict.Add(key, value);
                    }
                }
                return dict;
            }
            catch (Exception ex)
            {
                DomainEventLogger.LogDomainEvent("KV Error", new Dictionary<string, object>
                {
                    {"action", "GET" },
                    {"Error", ex.Message }
                });
                throw;
            }
        }
    }
}
