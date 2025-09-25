using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class PatientAppointmentsDTO
    {
        public  int Id { get; set; }
        public string AppointmentType { get; set; }
        public DateTime Date { get; set; }

    }
}
