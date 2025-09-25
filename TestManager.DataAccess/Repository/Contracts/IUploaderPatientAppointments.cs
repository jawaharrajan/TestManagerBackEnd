using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IUploaderPatientAppointments : IGenericRepository<Appointment, int>
    {
        Task<List<PatientAppointmentsDTO>> GetPatientAppointment(int patientId);
    }
}
