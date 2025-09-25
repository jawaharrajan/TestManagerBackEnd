namespace TestManager.Domain.Model.Uploader
{
    public partial class Prep_ReportingTeamTemplate : BaseEntity<int>
    {
        public int ReportingTeamTemplateId { get; set; }
        public required int ReportingTeamId { get; set; }
        public required Prep_ReportingTeam Prep_ReportingTeam { get; set; }
        public required int TemplateId { get; set; }
        public PrepTemplate PrepTemplate { get; set; }
    }
}
