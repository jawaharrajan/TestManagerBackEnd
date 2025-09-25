using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;

namespace TestManager.Service.Uploader
{
    public interface IPrepEducationMaterialService
    {
        Task<List<PrepEducationMaterialDTO>> GetEducationMaterial();
        Task<PrepEducationMaterialDTO?> GetEducationMaterialById(int educationMaterialId);
        Task<PrepEducationMaterialDTO> AddEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO);
        Task<PrepEducationMaterialDTO> UpdateEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO);
        Task<bool> DeleteEducationMaterial(int id);
    }
    public class PrepEducationMaterialService(IPrepEducationMaterialRepository prepEducationMaterialRepository) : IPrepEducationMaterialService
    {
        public async Task<List<PrepEducationMaterialDTO>> GetEducationMaterial()
        {
            return await prepEducationMaterialRepository.GetEducationMaterial();
        }
        public async Task<PrepEducationMaterialDTO?> GetEducationMaterialById(int educationMaterialId) 
        {
            return await prepEducationMaterialRepository.GetEducationMaterialById(educationMaterialId);
        }
        public async Task<PrepEducationMaterialDTO> AddEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO)
        {
            return await prepEducationMaterialRepository.AddEducationMaterial(prepEducationMaterialDTO);
        }
        public async Task<PrepEducationMaterialDTO> UpdateEducationMaterial(PrepEducationMaterialDTO prepEducationMaterialDTO)
        {
            return await prepEducationMaterialRepository.UpdateEducationMaterial(prepEducationMaterialDTO);
        }
        public async Task<bool> DeleteEducationMaterial(int id)
        {
            return await prepEducationMaterialRepository.DeleteEducationMaterial(id);
        }
    }
}
