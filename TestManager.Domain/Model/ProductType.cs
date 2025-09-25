namespace TestManager.Domain.Model;

public partial class ProductType : BaseEntity<int>
{
    public int ProductTypeId { get; set; }
    public string Name { get; set; }
}
