namespace TestManager.Domain.DTO
{
    public class ProductFilterDto
    {
        public int? DICOMModalityId { get; set; }
        public int? ProductTypeId { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
