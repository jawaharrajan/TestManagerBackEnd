using TestManager.Domain.DTO;
using TestManager.Domain.Model;


namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        Task<(List<ProductDTO> Products, int TotalCount)> GetProductsAsync(ProductFilterDto? filter = null);
        Task<ProductDTO> AddProduct(ProductDTO productDTO);
        Task<ProductDTO> UpdateProduct(ProductDTO productDTO);
        Task<List<ProductAddDTO>> GetProductstoAdd(int appointmentId);
        Task<bool> DeleteProduct(int Id);
    }
}
