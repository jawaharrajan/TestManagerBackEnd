
using TestManager.Domain.DTO.Uploader;

namespace TestManager.Domain.DTO
{
    public class AccuroLabObservationGroupDTO
    {
        public int TipsObservationGroupId { get; set; }
        public long? GroupId { get; set; }
        public long? BaseGroupId { get; set; }
        public long? Reviewer { get; set; }
        public string? ReviewerName { get; set; }
        public DateTime? ReviewDate { get; set; }
        public long? PatientId { get; set; }
        public long? TestId { get; set; }
        public long? SourceId { get; set; }
        public string? OrderProvider { get; set; }
        public string? UserNotes { get; set; }
        public string? UniversalSerialNum { get; set; }
        public string? SappId { get; set; }
        public string? FillerOrderNum { get; set; }
        public string? OrderGroup { get; set; }
        public DateTime? CollectionDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? TransferTipsDate { get; set; }
        public string? TestName { get; set; }
        public string? SourceName { get; set; }
        public bool? DoNotUpload { get; set; }
        public string? DoNotUploadNote { get; set; }
        public int? LetterId { get; set; }
        public DateTime? UploadedToMytestclient { get; set; }
        public DateTime? DoNotUploadDate { get; set; }
        public string? OrderProviderWithId { get; set; }
        public bool? ActiveVersion { get; set; }
        public IEnumerable<AccuroLabObservationDTO> Observations { get; set; }
        public bool Delayed48h { get; set; }
        public PrepLetterDTO? Letter { get; set; }
    }
}
