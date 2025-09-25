using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.TopLevelFilter
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string?  LocationName { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
    }
}
