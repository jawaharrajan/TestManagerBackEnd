using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class AdviceDTO
    {
        public int? AdviceId { get; set; }
        public int? PatientId { get; set; }
        public int? NurseCommunicationTypeId { get; set; }
        public string? Text { get; set; }
        public int? UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string? Result { get; set; }
        public int? AppointmentId { get; set; }
        public int StatusId { get; set; }
        public bool IsForNursingTab { get; set; }
    }
}
