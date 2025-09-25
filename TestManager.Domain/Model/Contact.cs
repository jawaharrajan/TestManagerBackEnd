namespace TestManager.Domain.Model;

public partial class Contact
{
    public int? Id { get; set; }

    public int? EntityTypeId { get; set; }

    public int? ContactTypeId { get; set; }

    public string? ContactValue { get; set; }

    public byte? IsPrimary { get; set; }

    public int? EntityId { get; set; }

    public byte? IsDeleted { get; set; }
}
