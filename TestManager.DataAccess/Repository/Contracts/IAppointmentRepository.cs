using TestManager.Domain.DTO;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAppointmentRepository : IGenericRepository<Appointment, int>
    {
        Task<(List<AppointmentDto> Appointments, int TotalCount)> GetAppointmentsForRadiologyAsync(AppointmentFilterDto filter);
        Task<(List<UploaderAppointmentDto> Appointments, int TotalCount)> GetAppointmentsForUploaderAsync(AppointmentFilterDto filter);
        Task<AppointmentDetailDto> GetAppointmentWithDetailsAsync(int Id);
        Task<List<ProductAddDTO>> GetProductsToAdd(int appointmentId);
    }
}
