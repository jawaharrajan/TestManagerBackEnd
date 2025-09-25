namespace TestManager.Domain.Model;

public partial class Prm_PaedsNursing
{
    public int? NursingId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public decimal? BodyMassIndex { get; set; }

    public decimal? HeightPercentile { get; set; }

    public decimal? WeightPercentile { get; set; }

    public decimal? BodyMassIndexPercentile { get; set; }

    public decimal? SystolicBloodPressure { get; set; }

    public decimal? DiastolicBloodPressure { get; set; }

    public decimal? HeartRate { get; set; }

    public string? Position { get; set; }

    public string? BodyPart { get; set; }

    public decimal? RestingECG { get; set; }

    public decimal? ElectronicMediaHoursDaily { get; set; }

    public decimal? BaselineConcussionTesting { get; set; }

    public string? Concerns { get; set; }

    public string? ProgressNotes { get; set; }

    public byte? ElectronicMedia { get; set; }

    public byte? HearingProtection { get; set; }

    public byte? HelmetSafety { get; set; }

    public byte? VehicleSafety { get; set; }

    public byte? WaterSafety { get; set; }

    public byte? SunSafety { get; set; }

    public byte? TrampolineSafety { get; set; }

    public byte? EnvironmentalHazards { get; set; }

    public byte? InternetSocialMedia { get; set; }

    public byte? Four_In_One_Vaccine { get; set; }

    public byte? Measles_Mumps_Rubella { get; set; }

    public byte? Meningococca { get; set; }

    public byte? Varicella { get; set; }

    public byte? Hepatitis_A { get; set; }

    public byte? Hepatitis_B { get; set; }

    public byte? Human_Papillomavirus { get; set; }

    public decimal? DoctorSystolicBloodPressure { get; set; }

    public decimal? DoctorDiastolicBloodPressure { get; set; }

    public string? SystolicBloodPressureRange { get; set; }

    public string? DiastolicBloodPressureRange { get; set; }

    public decimal? RestingElectrocardiogram { get; set; }

    public byte? Urinalysis { get; set; }

    public byte? ImmunizationUpToDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public byte? MeningococcalB { get; set; }
}
