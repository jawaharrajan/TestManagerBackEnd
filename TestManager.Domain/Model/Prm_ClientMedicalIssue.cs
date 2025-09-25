namespace TestManager.Domain.Model;

public partial class Prm_ClientMedicalIssue
{
    public int? ClientMedicalIssueId { get; set; }

    public int? MedicalIssueId { get; set; }

    public int? ApointmentId { get; set; }

    public int? PatientId { get; set; }

    public string? DoctorDescription { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? UserId { get; set; }

    public string? IssueName { get; set; }

    public int? OrderId { get; set; }
}
