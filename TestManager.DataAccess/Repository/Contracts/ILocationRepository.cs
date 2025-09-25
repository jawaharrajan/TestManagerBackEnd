using TestManager.Domain.DTO.TopLevelFilter;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface ILocationRepository : IGenericRepository<Locations, int>
    {
        Task<List<LocationDTO>> GetLocation();
    }
}
