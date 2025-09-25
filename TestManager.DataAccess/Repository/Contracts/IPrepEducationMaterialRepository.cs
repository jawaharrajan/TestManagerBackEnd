using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IPrepEducationMaterialRepository : IGenericRepository<Prep_EducationMaterial, int>
    {
        Task<List<PrepEducationMaterialDTO>> GetEducationMaterial();
        Task<PrepEducationMaterialDTO?> GetEducationMaterialById(int educationMaterialId);
        Task<PrepEducationMaterialDTO> AddEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO);
        Task<PrepEducationMaterialDTO> UpdateEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO);
        Task<bool>DeleteEducationMaterial(int id);
    }
}
