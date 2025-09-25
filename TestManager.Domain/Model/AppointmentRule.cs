namespace TestManager.Domain.Model
{
    public class AppointmentRule : BaseEntity<int>
    {
        //public required int Id { get; set; }  
		public int AppointmentTypeID { get; set; }
        public int ProductID { get; set; }
        public int? AccessionNoFlag  { get; set; }
        public bool? IsChairman { get; set; }  
        public bool IsFemale  { get; set; }  
        public bool IsMale  { get; set; }  
        public bool? SmokerRole  { get; set; }  
        public bool? NonSmokerRole  { get; set; }  
        public int? AgeFrom  { get; set; }  
        public int? AgeTo  { get; set; }  
        public int? AddDaysToAge  { get; set; }  
        public int? ToBeEvery  { get; set; }  
        public bool IsActive  { get; set; }  
    }
}
