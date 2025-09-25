namespace TestManager.Domain.Model;

public partial class PrepUploadReports
{
    public int? UploadReportId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public byte? Viewed { get; set; }

    public string? poc { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Nomytestclient { get; set; }

    public string? ACT { get; set; }

    public string? Genetic { get; set; }

    public byte? IsAccessible { get; set; }

    public byte? SentToCrawford { get; set; }

    public byte? ReceivedFromCrawford { get; set; }
}
