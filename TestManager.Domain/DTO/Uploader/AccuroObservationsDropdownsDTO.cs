using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class AccuroObservationsDropdownsDTO
    {
        public AccuroObservationsDropdownsDTO() 
        {
            this.Reviewers = [];
            this.ExternalLabs = [];
        }

        public Dictionary<long, string> Reviewers { get; set; }
        public Dictionary<long, string> ExternalLabs { get; set; }
    }
}
