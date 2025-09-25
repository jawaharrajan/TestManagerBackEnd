
using System.Collections;

namespace TestManager.Domain.DTO
{
    public class AccuroLabPatientCollectionDTO
    {
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? PhotoFile { get; set; }
        public long? PatientId { get; set; }
        public long? AccuroId { get; set; }
        public bool HasMytestclient { get; set; }
        public DateTime? CollectionDate { get; set; }
        public IEnumerable<AccuroLabOrdersSummary> OrdersSummary { get; set; } = [];
        public HashSet<string> Orders { get; set; } //FillerOrderNum

        public override int GetHashCode()
        {
            return HashCode.Combine(AccuroId, CollectionDate);
        }
    }

    public class AccuroLabOrdersSummary 
    {
        public string? ReviewerName { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? OrderProvider { get; set; }
        public string? SourceName { get; set; }
        public int? LetterId { get; set; }
        public bool? DoNotUpload { get; set; }
        public DateTime? AvailableOnMytestclient { get; set; }

        //string? FillerOrderNum { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReviewerName?.Trim(), ReviewDate.GetValueOrDefault().Date.ToString(), OrderProvider?.Trim(), SourceName?.Trim());
        }

        public override bool Equals(object? obj)
        {
            return GetHashCode() == obj?.GetHashCode();
        }
    }
}
