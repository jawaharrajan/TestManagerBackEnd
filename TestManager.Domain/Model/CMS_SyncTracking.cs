namespace TestManager.Domain.Model
{
    public class CMS_SyncTracking : BaseEntity<int>
    {
        public required string TableName { get; set; }
        public  int CMSID { get; set; }
        public int TIPSID { get; set; }
        public bool InboundProcessedFlag { get; set; }
    }
}
