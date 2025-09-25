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
    public class LocationRepository(ApplicationDbContext _) : GenericRepository<Locations, int>(_), ILocationRepository
    {
        public async Task<List<LocationDTO>> GetLocation()
        {
            string[] cities = { "Toronto", "Oakville" };
            return await _context.Location
                .AsNoTracking()
                .Where(l => cities.Contains(l.City))
                .Select(l => new LocationDTO
                {
                    Id = l.Id,
                    LocationName = l.City + "|" + l.Description
                })
                .ToListAsync();
        }
    }
}
