namespace TestManager.Domain.DTO
{
    public class AccuroLabObservationDTO
    {
      public int TipsObservationId { get; set; }
      public long? ObservationId { get; set; }
      public DateTime? ObservationDate { get; set; }
      public string? ObservationNote { get; set; }
      public string? ObservationFlag { get; set; }
      public string? ObservationValue { get; set; }
      public string? ObservationUnits { get; set; }
      public string? Label { get; set; }
      public long? ResultId { get; set; }
      public long? GroupId { get; set; }
      public string? ObservationReferenceRange { get; set; }
      public int? OrderNum { get; set; }
      public long? ObservationalSubIdNumber { get; set; }
      public string? ObsDisplayRefRange { get; set; }
      public string? ObservationalResultStatus { get; set; }
    }
}
