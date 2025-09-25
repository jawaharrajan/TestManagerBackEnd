namespace TestManager.Domain.Model.Uploader
{
    public class AccuroLabObservation : BaseEntity<int>
    {
      public int TipsObservationId { get; set; }// [tips_observation_id]
      public long? ObservationId { get; set; }// [observation_id]
      public DateTime? ObservationDate { get; set; }// [observation_date]
      public string? ObservationNote { get; set; }// [observation_note]
      public string? ObservationFlag { get; set; }// [observation_flag]
      public string? ObservationValue { get; set; }// [observation_value]
      public string? ObservationUnits { get; set; }// [observation_units]
      public string? Label { get; set; }// [label]
      public long? ResultId { get; set; }// [result_id]
      public long? GroupId { get; set; }// [group_id]
      public string? ObservationReferenceRange { get; set; }// [observation_reference_range]
      public int? OrderNum { get; set; }// [order_num]
      public long? ObservationalSubIdNumber { get; set; }// [observationalSubIdNumber]
      public string? ObsDisplayRefRange { get; set; }// [obsDisplayRefRange]
      public string? ObservationalResultStatus { get; set; }// [observationalResultStatus]
    }
}
