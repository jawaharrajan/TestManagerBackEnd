namespace TestManager.Domain.DTO.ActivityLog
{
    public class ActivityLogFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; } = string.Empty;
        public int InstanceId { get; set; } = 0;
        public int EntityTypeId { get; set; } = 0;
    }
}
