using TestManager.Domain.DTO;
using TestManager.Domain.DTO.ActivityLog;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog, int>
    {
        Task<(List<ActivityLogDTO> ActivityLogs, int TotalCount)> GetActivityLogsAsync(ActivityLogFilterDto filter);

    }
}
