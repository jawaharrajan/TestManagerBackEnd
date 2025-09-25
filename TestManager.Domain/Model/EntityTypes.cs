namespace TestManager.Domain.Model;

public partial class EntityTypes
{
    public int Id { get; set; }

    public int? Entity { get; set; }

    public string Description { get; set; } = null!;
}
