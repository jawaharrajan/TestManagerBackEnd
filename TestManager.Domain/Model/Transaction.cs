namespace TestManager.Domain.Model;

public partial class Transaction : BaseEntity<int>
{
    public int TransactionId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string? Reason { get; set; }

    public int? EntityTypeId { get; set; }

    public int? InstanceId { get; set; }

    public int? PatientId { get; set; }

    public int? UserId { get; set; }

    public int? AccountId { get; set; }

    // Navigation Property for logical relationship
    public Appointment? Appointment { get; set; }

    public ICollection<TransactionItem>? TransactionItems { get; set; } = new List<TransactionItem>();

    public ICollection<Invoice> Invoices { get; set; }
}
