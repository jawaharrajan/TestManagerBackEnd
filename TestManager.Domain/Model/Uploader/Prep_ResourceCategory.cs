namespace TestManager.Domain.Model.Uploader
{
    public class Prep_ResourceCategory : BaseEntity<int>
    {
        public int ConditionCategoryId { get; set; }
        public string? Description { get; set; }
        public bool InActive { get; set; }
    }
}
