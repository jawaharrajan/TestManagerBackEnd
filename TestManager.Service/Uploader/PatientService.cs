using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;

namespace TestManager.Service.Uploader
{
    public interface IPatientService
    {
        Task<(List<PatientDTO> Patients, int TotalCount)> GetPatientsAsync(PatientFilterDTO? filter = null);
        Task<PatientDTO?> GetPatientByIdAsync(int patientId);
    }

    public class PatientService(IPatientRepository patientRepository) : IPatientService
    {
        public async Task<(List<PatientDTO> Patients, int TotalCount)> GetPatientsAsync(PatientFilterDTO? filter = null)
        {
            var patients = await patientRepository.GetPatientsAsync(filter);
            return (patients.Patients, patients.TotalCount);
        }

        public async Task<PatientDTO?> GetPatientByIdAsync(int patientId) 
        {
            return await patientRepository.GetPatientByIdAsync(patientId);
        }

    }
}
