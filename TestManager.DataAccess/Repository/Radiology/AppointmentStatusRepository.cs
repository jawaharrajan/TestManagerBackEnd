using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class AppointmentStatusRepository(ApplicationDbContext _) : GenericRepository<Status, int>(_), IAppointmentStatusRepository
    {
        public async Task<List<AppointmentStatusDTO>> GetAppointmentStatusAsync()
        {
            var query = from s in _context.Status
                        where s.EntityTypeId == 3
                        select new AppointmentStatusDTO
                        {
                            StatusId =s.StatusId,
                            StatusName = s.Name ?? string.Empty
                        };

            return await query.ToListAsync();
        }
    }
}
