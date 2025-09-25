namespace TestManager.Domain.Model;

public partial class Prm_ClientAdditionalTest
{
    public int? ClientAdditionalTestId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public int? Test_Category { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? AdditionalTestId { get; set; }

    public string? Test_Results { get; set; }

    public int? TypeId { get; set; }

    public string? Reason { get; set; }

    public string? TimeFrame { get; set; }

    public int? PendingResultsId { get; set; }

    public string? LabRequisition { get; set; }

    public string? Test_Full_Name { get; set; }

    public int? OrderId { get; set; }
}
