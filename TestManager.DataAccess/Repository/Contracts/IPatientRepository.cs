using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IPatientRepository : IGenericRepository<Patient, int>
    {
        Task<(List<PatientDTO> Patients, int TotalCount)> GetPatientsAsync(PatientFilterDTO? patientFilterDTO = null);
        Task<PatientDTO?> GetPatientByIdAsync(int patientId);
    }
}
