namespace TestManager.Domain.Model;

public partial class Prm_FitnessHtmlReport
{
    public int? FitnessReportId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public string? ReportHTML { get; set; }

    public DateTime? CreateDate { get; set; }
}
