using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface INurseCommunicationTypeRepository : IGenericRepository<NurseCommunicationType,int>
    {
        Task<List<NurseCommunicationTypeDTO>> GetNurseCommunicationTypeAsync();
        Task<NurseCommunicationTypeDTO> AddNurseCommunicationTypeAsync(NurseCommunicationTypeDTO nurseCommunicationTypeDTO);
        Task<NurseCommunicationTypeDTO> UpdateNurseCommunicationTypeAsync(NurseCommunicationTypeDTO nurseCommunicationTypeDTO);
        Task<bool> DeleteNurseCommunicationTypeAsync(int id);
    }
}
