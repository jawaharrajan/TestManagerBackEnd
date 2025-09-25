using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.TopLevelFilter
{
    public class AppointmentTypeDTO
    {
        public int Id { get; set; }
        public string? AppointmentType { get; set; }
        public string? Code { get; set; }
    }
}
