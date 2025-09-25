namespace TestManager.Domain.Model.Uploader;

public partial class NurseCommunicationType : BaseEntity<int>
{
    public int NurseCommunicationTypeId { get; set; }
    public string? Description { get; set; }
    public bool? IsForNursingTab { get; set; }
    public  bool? IsForResultUpload { get; set; }
}

