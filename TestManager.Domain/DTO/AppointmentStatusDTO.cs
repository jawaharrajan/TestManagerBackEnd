using System.ComponentModel.DataAnnotations;


namespace TestManager.Domain.DTO
{
    public class AppointmentStatusDTO
    {
        public int StatusId { get; set; }

        [Required]
        public string? StatusName { get; set; }
    }
}
