namespace TestManager.Domain.Model;

public partial class Prm_Audit
{
    public int? AuditId { get; set; }

    public int? PatientId { get; set; }

    public int? AppointmentId { get; set; }

    public string? Description { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? UserId { get; set; }
}
