namespace TestManager.Domain.Model.UserManagement
{
    public class RoleReport : BaseEntity<int>
    {
        public int RoleID { get; set; }
        public string? Report { get; set; }
    }
}
