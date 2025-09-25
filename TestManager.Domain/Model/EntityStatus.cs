namespace TestManager.Domain.Model;

public partial class EntityStatus
{
    public int EntityStatusId { get; set; }

    public int EntityTypeId { get; set; }

    public int InstanceId { get; set; }

    public int StatusId { get; set; }

    public DateTime? CustomDate { get; set; }

    public int? WorkflowInstanceId { get; set; }

    public int? WorkflowId { get; set; }
}
