using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO
{
    public class ProductAddDTO
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public int ModialityId { get; set; }
        public int? ProductTypeId { get; set; }
    }
}
