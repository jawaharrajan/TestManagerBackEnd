using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class NurseCommunicationTypeRepository(ApplicationDbContext _) : GenericRepository<NurseCommunicationType, int>(_), INurseCommunicationTypeRepository
    {
        public async Task<List<NurseCommunicationTypeDTO>> GetNurseCommunicationTypeAsync()
        {
            var query = from n in _context.NurseCommunicationType
                        orderby n.Description
                        select new NurseCommunicationTypeDTO
                        {
                            NurseCommunicationTypeId = n.NurseCommunicationTypeId,
                            Description = n.Description,
                            IsForNursingTab = n.IsForNursingTab == true,
                            IsForResultUpload = n.IsForResultUpload == true
                        };

            return await query.ToListAsync();
        }

        public async Task<NurseCommunicationTypeDTO> AddNurseCommunicationTypeAsync(NurseCommunicationTypeDTO nurseCommunicationTypeDTO)
        {
            NurseCommunicationType nc = new()
            {                  
                Description = nurseCommunicationTypeDTO.Description,
                IsForNursingTab = nurseCommunicationTypeDTO.IsForNursingTab,
                IsForResultUpload = nurseCommunicationTypeDTO.IsForResultUpload
            };

            await AddAsync(nc);
            nurseCommunicationTypeDTO.NurseCommunicationTypeId = nc.NurseCommunicationTypeId;
            return nurseCommunicationTypeDTO;
        }

        public async Task<bool> DeleteNurseCommunicationTypeAsync(int id)
        {
            NurseCommunicationType? nc= await _context.NurseCommunicationType.FirstOrDefaultAsync(n => n.NurseCommunicationTypeId == id);

            if (nc == null) return false;

            _context.Remove(nc);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<NurseCommunicationTypeDTO?> UpdateNurseCommunicationTypeAsync(NurseCommunicationTypeDTO nurseCommunicationTypeDTO)
        {
            NurseCommunicationType? nc = await _context.NurseCommunicationType
                .FirstOrDefaultAsync(n => n.NurseCommunicationTypeId == nurseCommunicationTypeDTO.NurseCommunicationTypeId);

            if (nc == null) return null;

            nc.Description = nurseCommunicationTypeDTO.Description;
            nc.IsForNursingTab = nurseCommunicationTypeDTO.IsForNursingTab == true;
            nc.IsForResultUpload = nurseCommunicationTypeDTO.IsForResultUpload == true;
            await _context.SaveChangesAsync();
            return nurseCommunicationTypeDTO;

        }
    }
}
