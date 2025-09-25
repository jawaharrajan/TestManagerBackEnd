namespace TestManager.Domain.Model.Uploader;

public partial class PrepAttachment : BaseEntity<int>
{
    public int? AttachmentId { get; set; }

    public int? PatientId { get; set; }

    public int? InstanceId { get; set; }

    public int? LetterTypeId { get; set; }

    public int? LetterId { get; set; }

    public byte[] AttachmentPDF { get; set; }

    public string? Description { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? PassCode { get; set; }
    public string? Path { get; set; }
    public required PrepLetter Letter { get; set; }
    
}
