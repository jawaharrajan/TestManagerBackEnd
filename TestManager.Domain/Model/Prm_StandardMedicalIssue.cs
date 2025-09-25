namespace TestManager.Domain.Model;

public partial class Prm_StandardMedicalIssue
{
    public int? StandardMedicalIssue { get; set; }

    public int? IssueTypeId { get; set; }

    public string? IssueTypeName { get; set; }

    public string? IssueName { get; set; }

    public string? IssueDescription { get; set; }

    public string? IssueTypeOrder { get; set; }

    public string? IssueMappingName { get; set; }

    public string? IssueMappingValue { get; set; }
}
