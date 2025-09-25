using TestManager.DataAccess.Helper;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.ActivityLog;
using TestManager.Interfaces;

namespace TestManager.Service.ActivityLog
{
    public interface IActivityLogService
    {
        Task<(List<ActivityLogDTO> ActivityLogs, int TotalCount)> GetActivityLogsAsync(ActivityLogFilterDto filter);
        Task AddActivityLog(ActivityLogDTO activityLogDTO);
    }

    public class ActivityLogService(IActivityLogRepository activityLogRepository, IUserContextService userContextService) : IActivityLogService
    {
        public async Task<(List<ActivityLogDTO> ActivityLogs, int TotalCount)> GetActivityLogsAsync(ActivityLogFilterDto filter)
        {
            var activityLogs = await activityLogRepository.GetActivityLogsAsync(filter);
            return (activityLogs.ActivityLogs, activityLogs.TotalCount);
        }

        public async Task AddActivityLog(ActivityLogDTO activityLogDTO) 
        {
            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");
            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = activityLogDTO.SQLAction,
                EntityTypeId = activityLogDTO.EntityTypeId,
                InstanceId = activityLogDTO.InstanceId,
                EntityAction = activityLogDTO.EntityAction,
                UserEmail = userContextService.Email
            });
        }

    }
}
