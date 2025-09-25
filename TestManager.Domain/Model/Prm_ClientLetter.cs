namespace TestManager.Domain.Model;

public partial class Prm_ClientLetter
{
    public int? ClientLetterId { get; set; }

    public int? AppointmentId { get; set; }

    public string? ClientLetter { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? DoctorLetter { get; set; }

    public string? NutritionLetter { get; set; }

    public string? Signature { get; set; }
}
