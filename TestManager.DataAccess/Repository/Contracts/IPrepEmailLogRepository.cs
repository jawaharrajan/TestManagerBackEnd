using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IPrepEmailLogRepository : IGenericRepository<PrepEmailLog, int>
    {
        Task<IEnumerable<PrepEmailLogDTO>> GetEmailLogsByPatientId(int patientId);
    }
}
