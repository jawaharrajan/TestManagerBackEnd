using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class NurseCommunicationTypeDTO
    {
        public int NurseCommunicationTypeId { get; set; }
        public string? Description { get; set; }
        public bool? IsForNursingTab { get; set; }
        public bool? IsForResultUpload { get; set; }
    }
}
