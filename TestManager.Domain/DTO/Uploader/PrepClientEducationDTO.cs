namespace TestManager.Domain.DTO.Uploader
{
    public class PrepClientEducationDTO
    {
        public int ClientEducationId { get; set; }
        public int EducationMaterialId { get; set; }
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public bool Viewed { get; set; }
        public bool InActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public int? InActiveByUserId { get; set; }
    }

}
