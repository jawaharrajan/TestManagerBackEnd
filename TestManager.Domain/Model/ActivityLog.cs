namespace TestManager.Domain.Model
{
    public class ActivityLog : BaseEntity<int>
    {
        public DateTime ActivityDate { get; set; }
        public required string SQLAction { get; set; }
        public int EntityTypeId { get; set; }
        public int InstanceId { get; set; }
        public required string EntityAction { get; set; }
        public required string UserEmail { get; set; }

    }
}
