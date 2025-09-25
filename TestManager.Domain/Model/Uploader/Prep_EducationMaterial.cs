namespace TestManager.Domain.Model.Uploader
{
    public class Prep_EducationMaterial : BaseEntity<int>
    {
        public  int EducationMaterialId { get; set; }
        public string? Description { get; set; }
        public byte[]? Pdf { get; set; }
        public string? Url { get; set; }
        public string? Path { get; set; }
        public int ConditionCategoryId { get; set; }
        public int TypeId { get; set; }

        //public Prep_ResourceCategory? Prep_ResourceCategory { get; set; }
    }
}
