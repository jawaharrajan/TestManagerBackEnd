using TestManager.DataAccess.Helper;
using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.ActivityLog;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.AtivityLog
{
    public class ActivityLogRepository(ApplicationDbContext _) : GenericRepository<ActivityLog, int>(_), IActivityLogRepository
    {
        public async Task<bool> AddActivityLog(ActivityLogDTO activityLogDTO)
        {
            if (activityLogDTO == null) return false;

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            ActivityLog activityLog = new()
            {
                ActivityDate = estDate,
                SQLAction = "Insert",
                EntityTypeId = activityLogDTO.EntityTypeId,
                InstanceId = activityLogDTO.InstanceId,
                EntityAction = activityLogDTO.EntityAction,
                UserEmail = activityLogDTO.UserEmail
            };
            await _context.AddAsync(activityLog);

            return true;
        }

        public async Task<(List<ActivityLogDTO> ActivityLogs, int TotalCount)> GetActivityLogsAsync(ActivityLogFilterDto filter)
        {            

            var query = from al in _context.ActivityLog.AsNoTracking()
                        where (filter.InstanceId != 0 ? (al.InstanceId == filter.InstanceId) : true)
                        where (filter.EntityTypeId != 0 ? (al.EntityTypeId == filter.EntityTypeId) : true)                        
                        orderby al.ActivityDate descending
                        select new ActivityLogDTO
                        {
                            ActivityDate = al.ActivityDate,
                            EntityTypeId = al.EntityTypeId,
                            SQLAction = al.SQLAction,
                            EntityAction = al.EntityAction,                           
                            InstanceId = al.InstanceId,
                            UserEmail = al.UserEmail                            
                        };

            var totalCount = query.Count();

            var result = totalCount > 0
                ? await query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync()
                : new List<ActivityLogDTO>();

            return (result, totalCount);
        }

    }
}
