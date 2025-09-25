using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;

namespace TestManager.Service.Uploader
{
    public interface IPrepEmailLogService
    {
        Task<IEnumerable<PrepEmailLogDTO>> GetEmailLogsByPatientId(int patientId);
    }

    public class PrepEmailLogService(IPrepEmailLogRepository prepEmailLogRepository) : IPrepEmailLogService
    {
        public async Task<IEnumerable<PrepEmailLogDTO>> GetEmailLogsByPatientId(int patientId)
        {
            return await prepEmailLogRepository.GetEmailLogsByPatientId(patientId);
        }
    }
}
