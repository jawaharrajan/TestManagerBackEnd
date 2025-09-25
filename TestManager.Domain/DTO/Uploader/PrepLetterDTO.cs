using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class PrepLetterDTO
    {
        public int? LetterId { get; set; }

        public int? LetterTypeId { get; set; }

        public string? LetterName { get; set; }

        public string? Body { get; set; }

        public int? ReferralId { get; set; }

        public int? AppointmentId { get; set; }

        public int? PatientId { get; set; }

        public bool OpenLetter { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsAccessible { get; set; }

        public bool SentToCrawford { get; set; }

        public bool ReceivedFromCrawford { get; set; }
        public DateTime? AvailableOnMytestclient { get; set; }

        public IEnumerable<PrepAttachmentDTO> Attachments { get; set; }
        public DateTime? ViewedDate { get; set; }
        public int NurseCommunicationTypeId { get; set; } = 7;
    }
}
