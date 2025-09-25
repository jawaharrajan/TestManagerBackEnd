using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO
{
    public class AppointmentDto()
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }
        
        public IEnumerable<NoteDTO>? Notes { get; set; }
        //public string? Notes { get; set; }
        public DateTime Date { get; set; }

        public string? PhotoFile { get; set; }

        public int PID { get; set; }
        public bool HasMytestclient { get; set; }

        public required string PatientFirstName { get; set; }

        public required string PatientLastName { get; set; }

        public required string AppointmentType { get; set; }

        public required string AppointmentTypeCode { get; set; }

        public required string DoctorFullName { get; set; }

        public int? StatusId { get; set; }

        public required string StatusName { get; set; }

        public IEnumerable<TransactionItemDto>? TransactionItems { get; set; }
        public string? HealthCardNumber { get; set; }
        public string? HealthCardVersion { get; set; }
        public DateOnly? Birthdate { get; set; }
        public int? LocationId { get; set; }
        public byte? IsAppliedAutoProduct { get; set; }
        public string Gender { get; set; }
    }

}
