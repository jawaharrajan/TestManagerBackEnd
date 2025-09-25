using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Uploader;
using TestManager.Domain.DTO.Uploader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Service.Uploader
{
    public interface INurseCommunicationService
    {
        Task<List<NurseCommunicationTypeDTO>> GetNurseCommunicationTypeAsync();
        Task<NurseCommunicationTypeDTO> AddNurseCommunicationTypeAsync(NurseCommunicationTypeDTO prepTemplateDTO);
        Task<NurseCommunicationTypeDTO> UpdateNurseCommunicationTypeAsync(NurseCommunicationTypeDTO prepTemplateDTO);
        Task<bool> DeleteNurseCommunicationTypeAsync(int id);
    }
    public class NurseCommunicationService(INurseCommunicationTypeRepository nurseCommunicationTypeRepository) : INurseCommunicationService
    {
        public async Task<List<NurseCommunicationTypeDTO>> GetNurseCommunicationTypeAsync()
        {
            return await nurseCommunicationTypeRepository.GetNurseCommunicationTypeAsync();
        }

        public async Task<NurseCommunicationTypeDTO> AddNurseCommunicationTypeAsync(NurseCommunicationTypeDTO nurseCommunicationTypeDTO)
        {
            return await nurseCommunicationTypeRepository.AddNurseCommunicationTypeAsync(nurseCommunicationTypeDTO);
        }

        public async Task<NurseCommunicationTypeDTO> UpdateNurseCommunicationTypeAsync(NurseCommunicationTypeDTO nurseCommunicationTypeDTO)
        {
            return await nurseCommunicationTypeRepository.UpdateNurseCommunicationTypeAsync(nurseCommunicationTypeDTO);
        }

        public async Task<bool> DeleteNurseCommunicationTypeAsync(int id)
        {
            return await nurseCommunicationTypeRepository.DeleteNurseCommunicationTypeAsync(id);
        }
    }
}
