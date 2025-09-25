using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IPrepResourceRepository : IGenericRepository<Prep_ResourceCategory, int>
    {
        Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterials();

        Task<IEnumerable<PrepResourceDTO>> GetResourceWithEducationMaterialByResourceId(int resourceCategory);
    }
}
