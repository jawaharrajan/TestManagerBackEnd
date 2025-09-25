using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class AndrologistRepository(ApplicationDbContext _,
        ICMS_SyncTrackingRepository cms_SyncTrackingRepository) : GenericRepository<Andrologist, int>(_), IAndrologistRepository
    {
        public async Task<List<AndrologistDto>> GetAndrologistsAsync(AndrologistFilterDTO? filter = null)
        {
            var query = from d in _context.Andrologist
                        select new AndrologistDto
                        {
                            AndrologistId = d.AndrologistID,
                            FirstName = d.FirstName ?? string.Empty,
                            LastName = d.LastName ?? string.Empty,
                            Gender = d.Gender,
                            Address = d.Address
                        };

            #region - check for Filters
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(d =>
                        d.FirstName.Contains(filter.SearchTerm) ||
                        d.LastName.Contains(filter.SearchTerm));
                }
            }
            #endregion            

            return await query.ToListAsync();
        }

        public async Task<AndrologistDto> AddAndrologist(AndrologistDto andrologistDto)
        {            
            Andrologist andrologist = new()
            {                
                FirstName = andrologistDto.FirstName,
                LastName  = andrologistDto.LastName,
                Gender = andrologistDto.Gender,
                Address = andrologistDto.Address
            };

            await AddAsync(andrologist);
            andrologistDto.AndrologistId = andrologist.AndrologistID;
            return andrologistDto;
        }

        public async Task<AndrologistDto?> UpdateAndrologist(AndrologistDto andrologistDto)
        {
            //Check CMS tracking table if Andrologist record is processed in TIPS or not
            int tipsId = await cms_SyncTrackingRepository.GetTIPSId("Andrologist", andrologistDto.AndrologistId);

            Andrologist? andrologist = _context.Andrologist.FirstOrDefault(
                d => d.AndrologistID == tipsId);

            if (andrologist == null) return null;

            andrologist.FirstName = andrologistDto.FirstName;
            andrologist.LastName = andrologistDto.LastName;
            andrologist.Gender = andrologistDto.Gender;
            andrologist.Address = andrologistDto.Address;

            await _context.SaveChangesAsync();
            return andrologistDto;
        }

        public async Task<bool> DeleteAdnrologist(int Id)
        {
            //Check CMS tracking table if Andrologist record is processed in TIPS or not
            int tipsId = await cms_SyncTrackingRepository.GetTIPSId("Andrologist", Id);

            Andrologist? andrologist = _context.Andrologist
                .FirstOrDefault(d => d.AndrologistID == tipsId);

            if (andrologist == null) return false;
            
            _context.Remove(andrologist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
