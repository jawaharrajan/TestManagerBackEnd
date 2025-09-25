using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;
using TestManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Service.Uploader
{
    public interface IPrepTemplateService
    {
        Task<List<PrepTemplateDTO>> GetPrepTemplatesAysnc();        
        Task<PrepTemplateDTO> AddPrepTemplateAsync(PrepTemplateDTO prepTemplateDTO);
        Task<PrepTemplateDTO> UpdatePrepTemplateAsync(PrepTemplateDTO prepTemplateDTO);
        Task<bool> DeletePrepTemplateAsync(int id);
    }

    public class PrepTemplateService(IPrepTemplateRepository prepTemplateRepository,
                IActivityLogRepository activityLogRepository,
        IUserContextService userContextService) : IPrepTemplateService
    {

        public async Task<List<PrepTemplateDTO>> GetPrepTemplatesAysnc()
        {
            return await prepTemplateRepository.GetPrepTemplatesAysnc();
        }

        public async Task<PrepTemplateDTO> AddPrepTemplateAsync(PrepTemplateDTO prepTemplateDTO)
        {            
            return await prepTemplateRepository.AddPrepTemplateAsync(prepTemplateDTO);
        }

        public async Task<PrepTemplateDTO> UpdatePrepTemplateAsync(PrepTemplateDTO prepTemplateDTO)
        {
            return await prepTemplateRepository.UpdatePrepTemplateAsync(prepTemplateDTO);
        }

        public async Task<bool> DeletePrepTemplateAsync(int id)
        {
            return await prepTemplateRepository.DeletePrepTemplateAsync(id);
        }
    }
}
