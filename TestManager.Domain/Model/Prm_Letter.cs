namespace TestManager.Domain.Model;

public partial class Prm_Letter
{
    public int? DoctorLetterId { get; set; }

    public int? DoctorId { get; set; }

    public string? Letter { get; set; }

    public string? Nutrition { get; set; }

    public string? Signature { get; set; }

    public byte? Paediatric { get; set; }

    public int? AppointmentTypeId { get; set; }
}
