using TestManager.Domain.DTO;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAppointmentStatusRepository : IGenericRepository<Status, int>
    {
        Task<List<AppointmentStatusDTO>> GetAppointmentStatusAsync();
    }
}
