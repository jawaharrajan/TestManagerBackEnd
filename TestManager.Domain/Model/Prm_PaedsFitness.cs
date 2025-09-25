namespace TestManager.Domain.Model;

public partial class Prm_PaedsFitness
{
    public int? FitnessId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public string? HandGrip { get; set; }

    public string? StandingLongJump { get; set; }

    public string? FlexedArmHang { get; set; }

    public string? VerticalJump { get; set; }

    public string? TenMeterShuttleRun { get; set; }

    public string? Treadmill { get; set; }

    public decimal? HandGripValue { get; set; }

    public decimal? StandingLongJumpValue { get; set; }

    public decimal? FlexedArmHangValue { get; set; }

    public decimal? VerticalJumpValue { get; set; }

    public decimal? TenMeterShuttleRunValue { get; set; }

    public decimal? TreadmillValue { get; set; }

    public decimal? CurrentLevelActivity { get; set; }

    public decimal? Coordination { get; set; }

    public decimal? MuscularStrength { get; set; }

    public decimal? AerobicFitness { get; set; }

    public string? FitnessNotes { get; set; }

    public DateTime? CreateDate { get; set; }
}
