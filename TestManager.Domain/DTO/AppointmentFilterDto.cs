namespace TestManager.Domain.DTO
{
    public class AppointmentFilterDto
    {
        public int? StatusId { get; set; }
        public IEnumerable<int> AppointmentTypeIds { get; set; }
        public DateTime? AppointmentDate { get; set; } = DateTime.Today.Date;
        public string? SearchTerm { get; set; }
        public IEnumerable<int> LocationIds { get; set; } = [];
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "Date:asc";       
    }

}
