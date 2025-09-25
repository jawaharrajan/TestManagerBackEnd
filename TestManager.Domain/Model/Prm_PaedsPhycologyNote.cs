namespace TestManager.Domain.Model;

public partial class Prm_PaedsPhycologyNote
{
    public int? PhycologyNoteId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public string? Text { get; set; }

    public int? PhycologyNoteType { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreateDate { get; set; }
}
