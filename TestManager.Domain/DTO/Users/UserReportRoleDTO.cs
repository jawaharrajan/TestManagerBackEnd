namespace TestManager.Domain.DTO.Users
{
    public class UserReportRoleDTO
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public  string? RoleName { get; set; }
        public required string Report { get; set; }
    }
}
