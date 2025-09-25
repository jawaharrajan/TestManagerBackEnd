using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO
{
    public class UploaderAppointmentDto()
    {
        public int Id { get;  set; }
        public DateTime Date { get;  set; }
        public int PID { get;  set; }
        public string PhotoFile { get;  set; }
        public string PatientFirstName { get;  set; }
        public bool HasMytestclient { get;  set; }
        public string Gender { get;  set; }
        public string PatientLastName { get;  set; }
        public string HealthCardNumber { get;  set; }
        public string HealthCardVersion { get;  set; }
        public DateOnly? Birthdate { get;  set; }
        public string AppointmentType { get;  set; }
        public string AppointmentTypeCode { get;  set; }
        public int StatusId { get;  set; }
        public string StatusName { get;  set; }
        public bool HasLetters { get;  set; }
    }

}
