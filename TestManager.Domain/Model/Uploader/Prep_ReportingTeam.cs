namespace TestManager.Domain.Model.Uploader
{
    public partial class Prep_ReportingTeam : BaseEntity<int>
    {
        public int ReportingTeamId { get; set; }
        public required string ReportingTeamName { get; set; }
        public string EmailName { get; set; }
        public string Email { get; set; }
        public ICollection<Prep_ReportingTeamTemplate> TeamTemplates { get; set; } = [];
        public ICollection<Prep_ReportingTeamUser> TeamUsers { get; set; } = [];
    }
}
