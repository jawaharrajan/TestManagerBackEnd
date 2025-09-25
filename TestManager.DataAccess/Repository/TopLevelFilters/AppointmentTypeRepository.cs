using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.TopLevelFilter;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.TopLevelFilters
{
    public class AppointmentTypeRepository(ApplicationDbContext _) : GenericRepository<AppointmentType, int>(_), IAppointmentTypeRepository
    {
        public async Task<List<AppointmentTypeDTO>> GetAppointmentType()
        {
            return await _context.AppointmentType
                 .AsNoTracking()
                 .Select(at => new AppointmentTypeDTO
                 {
                     Id = at.Id,
                     AppointmentType = at.Name,
                     Code = at.Code
                 })
                 .ToListAsync();
        }
    }
}
