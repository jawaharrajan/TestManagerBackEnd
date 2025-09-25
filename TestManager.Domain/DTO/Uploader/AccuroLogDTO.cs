
using System.Collections;

namespace TestManager.Domain.DTO
{
    public class AccuroLogDTO
    {
        public string ResultType { get; set; }

        public DateTime? UploadedDate { get; set; }
        public DateTime? ViewedDate { get; set; }
        public bool? DoNotUpload { get; set; }
        public DateTime? DoNotUploadDate { get; set; }
        public bool? Delay48hours { get; set; }
        public int? LetterId { get; set; }
        public IEnumerable<OrderLogSummary> LogsSummary { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(LetterId, Delay48hours, DoNotUpload, DoNotUploadDate);
        }
        public override bool Equals(object? obj)
        {
            return obj.GetHashCode() == this.GetHashCode();
        }
    }

    public class OrderLogSummary 
    {
        public string? OrderedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string OrderId { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReviewedDate, OrderedBy);
        }
    }
}

