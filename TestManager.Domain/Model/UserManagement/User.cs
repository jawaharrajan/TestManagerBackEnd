namespace TestManager.Domain.Model.UserManagement;

public partial class User : BaseEntity<int>
{
    //public int Id { get; set; }
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserGoupd01 { get; set; }

    public string? UserGroup02 { get; set; }

    public string? UserGroup03 { get; set; }

    public string? UserGroup04 { get; set; }

    public string? UserGroup05 { get; set; }

    public string? JobTitle { get; set; }

    public string? Credentials { get; set; }

    public string? DirectAreaCode { get; set; }
    public string? DirectPhone { get; set; }

    public string? DirectExtension { get; set; }

    public string? Fax { get; set; }

    public string? Manager { get; set; }

    public string? Email { get; set; }

    public string? LongDistanceCode { get; set; }

    public string? UserName { get; set; }

    public int? WindowsLogId { get; set; }

    public string? AccuroUserName { get; set; }

    public ICollection<Note>? Notes { get; set; }
}
