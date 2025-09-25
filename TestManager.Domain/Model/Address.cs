namespace TestManager.Domain.Model;

public partial class Address
{
    public int? Id { get; set; }

    public int? EntityTypeId { get; set; }

    public string? BusinessName { get; set; }

    public string? Address01 { get; set; }

    public string? Address02 { get; set; }

    public string? City { get; set; }

    public string? ProvinceState { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public byte? IsPrimary { get; set; }

    public int? TypeId { get; set; }

    public byte? IsDeleted { get; set; }

    public byte? IsNew { get; set; }

    public int? EntityId { get; set; }
}
