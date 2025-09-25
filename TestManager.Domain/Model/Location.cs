namespace TestManager.Domain.Model
{
    public class Locations : BaseEntity<int>
    {
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
    }
}
