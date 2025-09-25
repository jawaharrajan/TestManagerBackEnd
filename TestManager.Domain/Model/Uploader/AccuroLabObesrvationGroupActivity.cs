namespace TestManager.Domain.Model.Uploader
{
    public class AccuroLabObservationResultsActivity : BaseEntity<int>
    {
        public int ObservationResultsLogId { get; set; }
        public int? PatientId { get; set; }
        public DateTime? CollectionDate { get; set; }
        public string? Activity { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserId { get; set; }
    }
}
