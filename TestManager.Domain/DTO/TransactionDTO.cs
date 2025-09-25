using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Reason { get; set; }
        public int InstanceId { get; set; }
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public List<TransactionItemDto>? TransactionItems { get; set; }
    }

}
