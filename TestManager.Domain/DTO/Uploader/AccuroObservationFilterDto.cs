
using TestManager.Enum;

namespace TestManager.Domain.DTO
{
    public class AccuroObservationFilterDto
    {
        public string? SearchTerm { get; set; }
        public AccuroObservationResultStatus ResultStatus { get; set; }
        public bool Pediatric { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public int? ExternalLab { get; set; }
        public int? ReviewedPhysician { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }       
    }

}
