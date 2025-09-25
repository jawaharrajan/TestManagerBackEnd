namespace TestManager.Domain.Model.Uploader
{
    public class PrepEmailLog : BaseEntity<int>
    {
        public int EmailLogId { get; set; }
        public int PatientId { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
    }
}
