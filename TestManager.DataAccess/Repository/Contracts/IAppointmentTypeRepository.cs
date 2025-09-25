using TestManager.Domain.DTO.TopLevelFilter;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAppointmentTypeRepository : IGenericRepository<AppointmentType, int>
    {
        Task<List<AppointmentTypeDTO>> GetAppointmentType();
    }
}
