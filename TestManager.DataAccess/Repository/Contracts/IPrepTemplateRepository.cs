using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IPrepTemplateRepository : IGenericRepository<PrepTemplate, int>
    {
        Task<List<PrepTemplateDTO>> GetPrepTemplatesAysnc();
        //Task<PrepTemplateDTO> GetPrepTemplateByIdAysnc(int id);
        Task<PrepTemplateDTO> AddPrepTemplateAsync(PrepTemplateDTO prepTemplateDTO);
        Task<PrepTemplateDTO?> UpdatePrepTemplateAsync(PrepTemplateDTO prepTemplateDTO);
        Task<bool> DeletePrepTemplateAsync(int id);
    }
}
