namespace TestManager.Domain.Model;

public partial class Prm_Fitness
{
    public int? FitnessId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public byte? FitnessNotDone { get; set; }

    public byte? BodPodNotDone { get; set; }

    public DateTime? CreateDate { get; set; }

    public byte? MeasurementsNotDone { get; set; }

    public string? BodyStation { get; set; }

    public byte? AerobicFitnessNotDone { get; set; }

    public byte? MovementsNotDone { get; set; }

    public byte? NutritionNotDone { get; set; }

    public string? Recommendations { get; set; }
}
