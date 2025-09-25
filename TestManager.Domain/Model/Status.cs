namespace TestManager.Domain.Model;

public partial class Status : BaseEntity<int>
{
    public int StatusId { get; set; }

    public int EntityTypeId { get; set; }

    public string? Name { get; set; }

    public byte? Default { get; set; }

    public byte? Active { get; set; }

    public string? mytestclientStatus { get; set; }
}
