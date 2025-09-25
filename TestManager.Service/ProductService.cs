using TestManager.DataAccess.Repository;
using TestManager.DataAccess.Repository.AtivityLog;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Service.Logging;
using TestManager.Domain.Model;
using TestManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestManager.DataAccess.Helper;

namespace TestManager.Service
{
    public interface IProductService
    {
        Task<(List<ProductDTO> Products, int TotalCount)> GetProductsAsync(ProductFilterDto? filter = null);
        Task<List<ProductAddDTO>> GetProductstoAdd(int appointmentId);
        Task<ProductDTO> AddProduct(ProductDTO productDTO);
        Task<ProductDTO> UpdateProduct(ProductDTO productDTO);
        Task<bool> DeleteProduct(int Id);
    }

    public class ProductService(IProductRepository productRepository,
        IActivityLogRepository activityLogRepository,
         IUserContextService userContextService) : IProductService
    {
        public async Task<(List<ProductDTO> Products, int TotalCount)> GetProductsAsync(ProductFilterDto? filter = null)
        {
            var (Products, TotalCount) = await productRepository.GetProductsAsync(filter);

            //await activityLogRepository.AddAsync(new ActivityLog
            //{
            //    ActivityDate = DateTime.Now,
            //    SQLAction = "Get",
            //    EntityTypeId = 40,
            //    InstanceId = 0,
            //    EntityAction = "Get All Products",
            //    UserEmail = userContextService.UserId
            //});

            DomainEventLogger.LogDomainEvent("GetProducts", new Dictionary<string, object>
            {                
                {"Action", "Get" },
                {"Products Count", TotalCount }
            });
            return (Products, TotalCount);
        }

        public async Task<List<ProductAddDTO>> GetProductstoAdd(int appointmentId)
        {
            var availableProducts = await productRepository.GetProductstoAdd(appointmentId);
            return availableProducts;
        }

        public async Task<ProductDTO> AddProduct(ProductDTO productDTO)
        {
            var result = await productRepository.AddProduct(productDTO);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Insert",
                EntityTypeId = 40,
                InstanceId = productDTO.ProductID,
                EntityAction = $"Add Product: {productDTO.ProductID}, Product Name: {productDTO.Name}",
                UserEmail = userContextService.Email
            });

            DomainEventLogger.LogDomainEvent("AddProduct", new Dictionary<string, object>
            {
                { "ProductId", result.ProductID },
                { "Action", "Insert" },
                { "ProductName", result.Name ?? "Unknown" }
            });
            return result;
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO productDTO)
        {
            var result = await productRepository.UpdateProduct(productDTO);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Update",
                EntityTypeId = 40,
                InstanceId = productDTO.ProductID,
                EntityAction = $"Update Product: {productDTO.ProductID}, Product Name: {productDTO.Name}",
                UserEmail = userContextService.Email
            });

            DomainEventLogger.LogDomainEvent("UpdateProduct", new Dictionary<string, object>
            {
                { "ProductId", result.ProductID },
                { "Action", "Update" },
                { "ProductName", result.Name ?? "Unknown" }
            });
            return result;
        }
        public async Task<bool> DeleteProduct(int Id)
        {
            var result = await productRepository.DeleteProduct(Id);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Delete",
                EntityTypeId = 40,
                InstanceId = Id,
                EntityAction = $"Delete Product: {Id}",
                UserEmail = userContextService.Email
            });

            DomainEventLogger.LogDomainEvent("DeleteProduct", new Dictionary<string, object>
            {
                { "ProductId", Id },
                { "Action", "Delete" }
            });
            return result;
        }
    }
}
