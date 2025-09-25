namespace TestManager.Domain.Model.UserManagement
{
    public class Role : BaseEntity<int>
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public bool System { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
