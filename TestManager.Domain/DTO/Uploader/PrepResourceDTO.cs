namespace TestManager.Domain.DTO.Uploader
{
    public class PrepResourceDTO
    {
        public int ConditionCategoryId { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }
        public ICollection<PrepEducationMaterialDTO> EducationMaterials { get; set; } = [];
    }

    public class PrepEducationMaterialDTO
    {
        public int EducationMaterialId { get; set; }
        public string Description { get; set; }
        public byte[]? Pdf { get; set; }
        public string Url { get; set; }
        public string? Path { get; set; }
        public int ConditionCategoryId { get; set; }
        public int TypeId { get; set; }
        public PrepResourceDTO ResourceCategory { get; set; }
        
    }

}
