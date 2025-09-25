namespace TestManager.Domain.Model;

public partial class ErrorLog
{
    public long? ErrorLogId { get; set; }

    public string? Method { get; set; }

    public string? Description { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? OrderId { get; set; }
}
