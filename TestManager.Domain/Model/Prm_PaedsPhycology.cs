namespace TestManager.Domain.Model;

public partial class Prm_PaedsPhycology
{
    public int? PhycologyId { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public decimal? WasiVerbalTscore { get; set; }

    public decimal? WasiPerceptualTscore { get; set; }

    public decimal? WasiVerbalPercentile { get; set; }

    public decimal? WasiPerceptualPercentile { get; set; }

    public decimal? WecshlerReadingComprehensionTscore { get; set; }

    public decimal? WecshlerSpellingTscore { get; set; }

    public decimal? WecshlerWordReadingTscore { get; set; }

    public decimal? WecshlerMathSkillsTscore { get; set; }

    public decimal? WechslerReadingComprehensionSubtestTscore { get; set; }

    public decimal? WecshlerSpellingPercentile { get; set; }

    public decimal? WechslerWordReadingPercentile { get; set; }

    public decimal? WecshlerMathSkillsPercentile { get; set; }

    public decimal? WecshlerReadingCompositePercentile { get; set; }

    public string? BascExternalizingProblems { get; set; }

    public string? BascInternalizingProblems { get; set; }

    public decimal? BascBehavioralSymptomIndex { get; set; }

    public decimal? BascAdaptiveSkills { get; set; }

    public decimal? BascExternalizingProblemsPercentile { get; set; }

    public decimal? BascInternalizingProblemsPercentile { get; set; }

    public decimal? BascBehavioralSymptomIndexPercentile { get; set; }

    public decimal? BascAdaptiveSkillsPercentile { get; set; }

    public decimal? SelfReportSchoolProblems { get; set; }

    public decimal? SelfReportInternalizingProblems { get; set; }

    public decimal? SelfReportInattentionHyperactivity { get; set; }

    public decimal? SelfReportEmotionalSymptomIndex { get; set; }

    public decimal? SelfReportPersonalAdjustment { get; set; }

    public decimal? SelfReportSchoolProblemsPercentile { get; set; }

    public decimal? SelfReportInternalizingProblemsPercentile { get; set; }

    public decimal? SelfReportInattentionHyperactivityPercentile { get; set; }

    public decimal? SelfReportEmotionalSymptomIndexPercentile { get; set; }

    public decimal? SelfReportPersonalAdjustmentPercentile { get; set; }

    public decimal? ParentScore { get; set; }

    public decimal? ChildScore { get; set; }

    public string? WechslerNote { get; set; }

    public string? SelfReportNote { get; set; }

    public string? PertinentObservation { get; set; }

    public string? Conclusion { get; set; }

    public string? Recommendation { get; set; }

    public DateTime? CreateDate { get; set; }
}
