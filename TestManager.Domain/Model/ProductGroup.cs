namespace TestManager.Domain.Model;

public partial class ProductGroup : BaseEntity<int>
{
    public int ProductGroupId { get; set; }
    public string ProductGroupName { get; set; }
    public int? MasterGroupId { get; set; }
}
