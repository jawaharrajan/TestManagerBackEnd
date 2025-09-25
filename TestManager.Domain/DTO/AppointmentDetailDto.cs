using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO
{
    public class AppointmentDetailDto
    {
        public int Id { get; set; }

        public string? ArrivalNotes { get; set; }

        public DateTime Date { get; set; }

        public List<TransactionDto>? Transactions { get; set; }

        public string? PhotoFile { get; set; }

        public int PID { get; set; }

        [Required]
        public string PatientFirstName { get; set; }

        [Required]
        public string PatientLastName { get; set; }

        [Required]
        public string AppointmentType { get; set; }

        [Required]
        public string AppointmentTypeCode { get; set; }

        [Required]
        public string DoctorFullName { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public string StatusName { get; set; }

    }
}
