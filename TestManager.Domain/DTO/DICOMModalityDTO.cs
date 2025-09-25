namespace TestManager.Domain.DTO
{
    public class DICOMModalityDTO
    {
        public int ModalityId { get; set; }
        public required string StudyDescription { get; set; }
        public required string RoomCode { get; set; }
        public string ProcedureCode { get; set; } = string.Empty;
        public string ModalityCode { get; set; } = string.Empty;
    }
}
