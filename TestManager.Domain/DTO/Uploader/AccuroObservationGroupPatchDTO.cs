using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class AccuroObservationGroupPatchDTO
    {
        public IEnumerable<string> Orders { get; set; }
        public int? LetterId { get; set; }
        public bool DoNotUpload { get; set; }
        public string? DoNotUploadNote { get; set; }
        public  DateTime? CollectionDate { get; set; }
        public int? PatientId { get; set; }
        public string? Activity { get; set; }   
    }
}
