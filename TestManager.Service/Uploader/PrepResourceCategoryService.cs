using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;

namespace TestManager.Service.Uploader
{
    public interface IPrepResourceCategoryService
    {
        Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterials();
        Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterialByResourceId(int resourceCategory);
    }
    public class PrepResourceCategoryService(IPrepResourceRepository prepResourceRepository) : IPrepResourceCategoryService
    {
        public async Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterials()
        {
            return await prepResourceRepository.GetResourceWithEducationMaterials();
        }
        public async Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterialByResourceId(int resourceCategory)
        {
            return await prepResourceRepository.GetResourceWithEducationMaterialByResourceId(resourceCategory);
        }
    }
    
}
