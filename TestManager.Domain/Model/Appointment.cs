using System;
using System.Collections.Generic;

namespace TestManager.Domain.Model;

public partial class Appointment : BaseEntity<int>
{
    //public int Id { get; set; }

    public int AppointmentTypeId { get; set; }

    public string? DateCreated { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public string? ProviderId { get; set; }

    public string? NonClinic { get; set; }

    public string? VIP { get; set; }

    public string? PayOnArrival { get; set; }

    public string? BookedByUserId { get; set; }

    public string? SoldByUserId { get; set; }

    public string? Reason { get; set; }

    public string? AdditionalConfirmationText { get; set; }

    public string? ConfirmationSent { get; set; }

    public int LocationId { get; set; }

    public string? ArrivalTime { get; set; }

    public string? ArrivalNotes { get; set; }

    public string? ExitTime { get; set; }

    public string? TotalElapsed { get; set; }

    public string? Cancelled { get; set; }

    public string? RoomId { get; set; }

    public string? NurseId { get; set; }

    public string? AppointmentStyle { get; set; }

    public string? BadExperience { get; set; }

    public string? PreviousBadExperience { get; set; }

    public string? FirstTime { get; set; }

    public string? PaymentCoverage { get; set; }

    public string? PaymentRequired { get; set; }

    public string? Paid { get; set; }

    public string? StatusIcon { get; set; }

    public string? StatusIconTimer { get; set; }

    public string? ReferrerDoctorId { get; set; }

    public string? VoluntaryReschedules { get; set; }

    public string? Scheduled { get; set; }

    public string? LastUpdated { get; set; }

    public string? PrepPacketSent { get; set; }

    public string? HSID { get; set; }

    public string? Complaint { get; set; }

    public string? ComplaintNotes { get; set; }

    public string? ClinicId { get; set; }

    public string? ReminderEmailAddendum { get; set; }

    public string? ReferralContactByPhone { get; set; }

    public string? OriginalDate { get; set; }

    public string? mytestclientView { get; set; }

    public byte? IsAppliedAutoProduct { get; set; }

    public string? InitialCommunication { get; set; }

    public string? OptimisticLockField { get; set; }

    public string? WaitTimeExtremeTimeout { get; set; }

    public string? WaitTImeWarningTimeout { get; set; }

    public string? PhoneConsult { get; set; }

    public string? ApptTypeChangeDate { get; set; }

    public string? OnlineBooking { get; set; }

    public string? AccuroId { get; set; }

    public string? Province { get; set; }

    public string? IsAccessibleAvailable { get; set; }

    public string? CreatedExitMSTrans { get; set; }

    public string? InvoluntaryReschedules { get; set; }

    public string? ScheduleID { get; set; }

    public DateTime Date { get; set; }

    // Navigation Property for related Products
    public Transaction Transaction { get; set; } = new();

    public Patient Patient { get; set; } = new();

    public ICollection<Note>? Notes { get; set; } = new List<Note>();

}
