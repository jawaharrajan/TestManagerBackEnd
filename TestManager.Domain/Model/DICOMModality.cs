namespace TestManager.Domain.Model;

public partial class DICOMModality : BaseEntity<int>
{
    public int DICOMModalityId { get; set; }

    public string? RoomCode { get; set; }

    public string? StudyDescription { get; set; }

    public string? ProcedureCode { get; set; }

    public string? ModalityCode { get; set; }

    //public bool IsDeleted { get; set; }
}
