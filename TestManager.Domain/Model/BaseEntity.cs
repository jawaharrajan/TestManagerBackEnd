namespace TestManager.Domain.Model
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; } //Generic Id Field
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
