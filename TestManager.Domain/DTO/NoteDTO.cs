using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO
{
    public class NoteDTO
    {
        public int NoteID { get; set; }
        public int EntityTypeID { get; set; }
        public int InstanceID { get; set; } = 0;
        public string? Text { get; set; } = string.Empty;
        public int? UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int NoteType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}