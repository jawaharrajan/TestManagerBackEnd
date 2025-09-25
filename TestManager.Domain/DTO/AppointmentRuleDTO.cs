namespace TestManager.Domain.DTO
{
    public class AppointmentRuleDTO
    {
        public int Id { get; set; }
        public int AppointmentTypeId { get; set; }
        public string? AppointmentTypeName { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int AccessionNoFlag { get; set; }
        public bool IsChairman { get; set; }
        public bool IsFemale { get; set; }
        public bool IsMale { get; set; }
        public bool SmokerRole { get; set; }
        public bool NonSmokerRole { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public int AddDaysToAge { get; set; }
        public int ToBeEvery { get; set; }
        public bool IsActive { get; set; }
    }
}
