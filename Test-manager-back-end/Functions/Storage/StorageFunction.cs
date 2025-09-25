using Azure.Storage.Sas;
using Azure.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Storage;

public class StorageFunction (ILogger<StorageFunction> logger, IConfiguration config) : BaseFunction(logger, config)
{
    [Function("GetUploadUrl")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "storage")] HttpRequestData req)
    {
        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string fileName = query["filename"] ?? throw new ArgumentNullException("filename");
        string mode = query["mode"] ?? throw new ArgumentNullException("mode");
        int tokenExpiration = int.TryParse(config["TokenExpiration"], out var expiration) ? expiration : 1;
        //var configs = await GetKeyVaultSecrets(["StorageAccountName", "StorageAccountKey", "ContainerName"]);

        return await ExecuteSafeAsync(async () =>
        {
            string accountName = config["StorageAccountName"];
            string accountKey = config["StorageAccountKey"];
            string container = config["ContainerName"];


            var credential = new StorageSharedKeyCredential(accountName, accountKey);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = container,
                BlobName = fileName,
                Resource = "b", // "b" for blob
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(tokenExpiration)
            };

            if (mode == "read") {
                sasBuilder.SetPermissions(BlobSasPermissions.Read);
            }
            else if (mode == "write")
            {
                sasBuilder.SetPermissions(BlobSasPermissions.Create | BlobSasPermissions.Write);
            }
            
            var sasToken = sasBuilder.ToSasQueryParameters(credential).ToString();
            var blobUrl = $"https://{accountName}.blob.core.windows.net/{container}/{fileName}?{sasToken}";
            return blobUrl;
        }, string.Empty);
    }
}