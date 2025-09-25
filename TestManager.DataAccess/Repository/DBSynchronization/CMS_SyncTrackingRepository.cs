using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.DBSynchronization
{
    public class CMS_SyncTrackingRepository(ApplicationDbContext _) : GenericRepository<CMS_SyncTracking, int>(_), ICMS_SyncTrackingRepository
    {
        public async Task<int> GetTIPSId(string tableName, int cmsId)
        {
            var tipsId = from c in _context.CMS_SyncTracking
                         where c.InboundProcessedFlag == true && c.CMSID == cmsId && c.TableName == tableName
                         select c.TIPSID;

            if (tipsId.Any())
            {
                return await tipsId.FirstAsync();
            }
            return cmsId;
        }

        public async Task<int> GetTIPSIdNoFlag(string tableName, int cmsId)
        {
            var tipsId = from c in _context.CMS_SyncTracking
                         where c.CMSID == cmsId && c.TableName == tableName
                         select c.TIPSID;

            if (tipsId.Any())
            {
                return await tipsId.FirstAsync();
            }
            return cmsId;
        }
    }
}
