namespace TestManager.Domain.Model
{
    public class Andrologist : BaseEntity<int>
    {
        public int AndrologistID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Gender { get; set; }
        public string? Address { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
