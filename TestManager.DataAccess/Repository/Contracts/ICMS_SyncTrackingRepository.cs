using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface ICMS_SyncTrackingRepository : IGenericRepository<CMS_SyncTracking, int>
    {
        Task<int> GetTIPSId(string tableName, int cmsId);
        Task<int> GetTIPSIdNoFlag(string tableName, int cmsId);
    }
}
