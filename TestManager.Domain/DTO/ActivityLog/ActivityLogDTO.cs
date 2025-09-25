namespace TestManager.Domain.DTO.ActivityLog
{
    public class ActivityLogDTO
    {
        public int Id { get; set; }
        public DateTime ActivityDate { get; set; }        
        public int EntityTypeId { get; set; }
        public int InstanceId { get; set; }
        public required string SQLAction { get; set; }
        public required string EntityAction { get; set; }
        public required string UserEmail { get; set; }
    }
}
