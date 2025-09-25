namespace TestManager.Domain.Model
{
    public class SequenceValue : BaseEntity<int>
    {
        public int NextVal { get; set; }
    }
}
