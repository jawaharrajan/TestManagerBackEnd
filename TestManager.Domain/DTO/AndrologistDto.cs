namespace TestManager.Domain.DTO
{
    public class AndrologistDto
    {
        public int AndrologistId { get; set; }
        public required string FirstName  { get; set; }
        public required string  LastName { get; set; }
        public required string Gender { get; set; }
        public string?  Address { get; set; }
    }
}
