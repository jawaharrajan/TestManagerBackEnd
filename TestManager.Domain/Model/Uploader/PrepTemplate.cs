namespace TestManager.Domain.Model.Uploader
{
    public partial class PrepTemplate : BaseEntity<int>
    {
        public  int TemplateId { get; set; }
        public  string? Description { get; set; }
        public string? Subject1 { get; set; }
        public string? Subject2 { get; set; }
        public string? Text { get; set; }
        public  int? TypeId { get; set; }
        public int? BlurbTypeId { get; set; }
        public bool? Inactive { get; set; }
    }
}
