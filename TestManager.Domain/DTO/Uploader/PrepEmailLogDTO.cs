using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class PrepEmailLogDTO
    {
        public int EmailLogId { get; set; }
        public int PatientId { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
    }
}
