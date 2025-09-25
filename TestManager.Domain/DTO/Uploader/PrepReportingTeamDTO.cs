namespace TestManager.Domain.DTO.Uploader
{
    public class ReportingTeamDTO
    {
        public int ReportingTeamId { get; set; }
        public required string ReportingTeamName { get; set; }
        public List<PrepReportingTeamTemplateDTO> PrepReportingTeamTemplate { get; set; }
        public  List<PrepReportingTeamUserDTO> PrepReportingTeamUsers { get; set; }
        public string EmailName { get; set; }
        public string Email { get; set; }
    }

    public partial class PrepReportingTeamTemplateDTO
    {
        public int ReportingTeamTemplateId { get; set; }
        public int ReportingTeamId { get; set; }
        public required int TemplateId { get; set; }

        //public required PrepTemplateDTO PrepTemplate { get; set; }        
    }

    public partial class PrepTemplateDTO
    {
        public int? TemplateId { get; set; }
        public string? Description { get; set; }
        public string? Text { get; set; }
        public int? TypeId { get; set; }
        public string? Subject1 { get; set; }
        public string? Subject2 { get; set; }
    }

    public partial class PrepReportingTeamUserDTO
    {
        public int ReportingTeamUserId { get; set; }
        public int ReportingTeamId { get; set; }
        public int UserId { get; set; }
    }
}
