namespace TestManager.Domain.Model.UserManagement
{
    public class UserRole : BaseEntity<int>
    {
        public int UserRoleId { get; set; }
        public int RoleID { get; set; }
        public int UserID { get; set; }
    }
}
