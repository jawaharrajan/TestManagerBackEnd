using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Service.Helper;
using TestManager.Domain.Model;
using TestManager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Radiology
{
    public class ProductsFunction(IGenericRepository<Product, int> productRepository, 
        IGenericRepository<ProductType, int> productTypeRepository,
        IGenericRepository<ProductGroup, int> productGroupRepository,
        IProductService productService,
        ILogger<ProductsFunction> logger) : BaseFunction(logger)
    {
        [Function("GetProducts")]
        public async Task<IActionResult> GetProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "product")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Fetching all Products");
            var filter = new ProductFilterDto();
            if (req.QueryString.HasValue)
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
                filter = new ProductFilterDto
                {
                    DICOMModalityId = int.TryParse(query["modalityId"], out var modId) ? modId : null,
                    ProductTypeId = int.TryParse(query["productTypeId"], out var productTypeId) ? productTypeId : null,
                    SearchTerm = query["searchTerm"],
                    Page = int.TryParse(query["page"], out var p) ? p : 1,
                    PageSize = int.TryParse(query["pageSize"], out var ps) ? ps : 10,
                };
            }

            return await ExecutePagedAsync<ProductDTO>(
               async () =>
               {
                   var (data, total) = await productService.GetProductsAsync(filter);
                   return (data, total);
               }, filter.Page, filter.PageSize, "Get All Products");
        }

        [Function("GetProductTypes")]
        public async Task<OkObjectResult> GetProductTypes([HttpTrigger(AuthorizationLevel.Function, "get", Route = "productTypes")] HttpRequest req)
        {

            logger.LogInformation("Fetching all Product Types");

            var productTypes = await productTypeRepository.GetAllAsync();

            logger.LogInformation($"Retrieved {productTypes.Count()} Products");

            return new OkObjectResult(productTypes);
        }

        [Function("GetProductGroups")]
        public async Task<OkObjectResult> GetProductGroups([HttpTrigger(AuthorizationLevel.Function, "get", Route = "productGroups")] HttpRequest req)
        {
            
            logger.LogInformation("Fetching all Product Groups");

            var productGroups = await productGroupRepository.GetAllAsync();

            logger.LogInformation($"Retrieved {productGroups.Count()} Product Groups");

            return new OkObjectResult(productGroups);
        }

        [Function("AddProduct")]
        public async Task<IActionResult> AddProduct([HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Add a new Product");

            var product = await req.ReadFromJsonAsync<ProductDTO>();

            if (product is null || string.IsNullOrEmpty(product.Name)){
                return new BadRequestObjectResult(
                   new ApiResponse<string>("Invalid payload: Product cannot be null, Product Name is required.", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var newProdcut = await productService.AddProduct(product);
                    return newProdcut;
                }, "Product added successfully"); ;
        }

        [Function("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([HttpTrigger(AuthorizationLevel.Function, "put", Route = "product")] HttpRequest req)
        {
            // EnrichLoggingFromRequest(req, enricher);
            logger.LogInformation("Add a new Product");

            var product = await req.ReadFromJsonAsync<ProductDTO>();

            if (product is null || string.IsNullOrEmpty(product.Name))
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: Product cannot be null, Product Name is required.", false));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var updateProdcut = await productService.UpdateProduct(product)
                        ?? throw new KeyNotFoundException($"Product with Id: {product.ProductID} Not found");
                    return updateProdcut;
                }, "Product updated successfully");
        }

        [Function("DeleteProduct")]

        public async Task<IActionResult> DeleteProduct([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product/{Id}")] HttpRequest req,
            int id)
        {
            // EnrichLoggingFromRequest(req, enricher);
            ProductFilterDto filter = new ProductFilterDto();
            if (id <= 0)
            {
                return new BadRequestObjectResult(
                    new ApiResponse<string>("Invalid payload: ProductID is required.", false));
            }

            return await ExecutePagedAsync<ProductDTO>(
               async () =>
               {
                   var deleted = await productService.DeleteProduct(id);
                   if (!deleted)
                   {
                       throw new KeyNotFoundException($"Andrologist with ID {id} not found.");
                   }

                   var (data, total) = await productService.GetProductsAsync(filter);
                   return (data, total);
               },
               filter.Page, filter.PageSize, $"Deleted Product with Id: {id} successfully."
            );
        }
    }
}
