using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class PrepAttachmentDTO
    {
        public int? AttachmentId { get; set; }

        public int? PatientId { get; set; }

        public int? InstanceId { get; set; }

        public int? LetterTypeId { get; set; }

        public int? LetterId { get; set; }

        public byte[]? AttachmentPDF { get; set; }

        public string? Description { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? PassCode { get; set; }

        public string? Filename { get; set; }
        public string? Path { get; set; }
    }
}
