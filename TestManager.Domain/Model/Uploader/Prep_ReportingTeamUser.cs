namespace TestManager.Domain.Model.Uploader
{
    public class Prep_ReportingTeamUser : BaseEntity<int>
    {
        public int ReportingTeamUserId { get; set; }
        public int ReportingTeamId { get; set; }
        public int UserId { get; set; }
        public Prep_ReportingTeam ReportingTeam { get; set; }
    }
}
