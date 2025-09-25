namespace TestManager.Domain.Model;

public partial class AppointmentType : BaseEntity<int>
{
    //public int Id { get; set; }

    public string? RecallApptTypeId { get; set; }

    public string? DontCloseRecallsIfCantContact { get; set; }

    public string? ApptGroupId { get; set; }

    public string? ApptBackColorId { get; set; }

    public string? ApptForeColorId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Active { get; set; }

    public string? RequiresReason { get; set; }

    public string? RequiresProduct { get; set; }

    public string? RequiresProductType { get; set; }

    public string? RequiresMedicalReport { get; set; }

    public string? TriggersTentativeAppointment { get; set; }

    public string? CheckForReferrals { get; set; }

    public string? CreateDate { get; set; }

    public string? UpdateDate { get; set; }

    public string? HSID { get; set; }

    public string? LinkedToCha { get; set; }

    public string? EnhancedPatientManagerInterface { get; set; }

    public string? RecallTimeline { get; set; }

    public string? RecallTimeline_PrivPhone { get; set; }

    public string? RecallTimeline_PrivVoiceMail { get; set; }

    public string? RecallTimeline_PrivEmail { get; set; }

    public string? RecallTimeline_PrivPostalMail { get; set; }

    public string? RecallTimeline_CorpPhone { get; set; }

    public string? RecallTimeline_CorpVoiceMail { get; set; }

    public string? RecallTimeline_CorpEmail { get; set; }

    public string? RecallTimeline_CorpPostalMail { get; set; }

    public string? BookingEmailId { get; set; }

    public string? ReminderEmailId { get; set; }

    public string? ReminderEmailLeadTime { get; set; }

    public string? ReminderEmailCalendarDays { get; set; }

    public string? ReminderEmailFallback { get; set; }

    public string? BookingEmailFallback { get; set; }

    public string? LISBarCode { get; set; }

    public string? LearnMore { get; set; }

    public string? Preparation { get; set; }

    public string? Floor { get; set; }

    public string? AutoAddNRF { get; set; }

    public string? AutoEmail { get; set; }

    public string? TimeMins { get; set; }

    public string? SearchApptGroup { get; set; }

    public string? TriggersRecallTask { get; set; }

    public string? mytestclient { get; set; }

    public string? AddOn { get; set; }

    public string? PMArrivalStation { get; set; }

    public string? IsParentAppt { get; set; }

    public string? WaitingTimeExtremeTimeout { get; set; }

    public string? WaitingTimeWarningTimeout { get; set; }

    public string? TotalScheduleSlots { get; set; }

    public string? IsCHA { get; set; }

    public string? AbleToCancelledHours { get; set; }

    public string? OnlineBooking { get; set; }

    public string? OnlineBooking_Name { get; set; }

    public string? OnlineBooking_DoctorRequired { get; set; }

    public string? OnlineBooking_ProductId { get; set; }

    public string? OnlineBooking_ReasonRequired { get; set; }

    public string? OnlineBooking_DoctorId { get; set; }

    public string? OnlineBooking_Description { get; set; }

    public string? AppointmentTypeSubGroupId { get; set; }

    public string? Point { get; set; }

    public string? CancellationFee { get; set; }

    public string? SecondEmailReminder { get; set; }

    public string? OrigCode { get; set; }

    public string? AccuroId { get; set; }

    public string? AccuroOfficeId { get; set; }

    public string? IsSearchable { get; set; }

    public string? OnlineBooking_DoctorGender { get; set; }

    public string? OnlineBooking_InitialAppointmentType { get; set; }

    public string? OnlineBooking_InitialAppointmentMonthInterval { get; set; }

    public string? OnlineBooking_FollowupAppointmentType { get; set; }

    public string? OnlineBooking_InitialIncludeFollowup { get; set; }

    public string? OnlineBooking_InitialIncludeFollowupName { get; set; }
}
