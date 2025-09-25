namespace TestManager.Domain.DTO.Users
{
    public class UserRoleDTO
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public required string RoleName { get; set; }
    }
}
