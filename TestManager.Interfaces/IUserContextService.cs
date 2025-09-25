using TestManager.Domain.DTO.Users;
namespace TestManager.Interfaces
{
    public interface IUserContextService
    {
        string? Email { get; set; }
        string? FirstName { get; set; }
        string? LastName { get; set; }
        int TipsUserId { get; set; }

        void GetUserId();
        Task GetUserIdAsync();
        Task<UserDTO> GetUserDetails(UserDTO userDTO);
        Task<List<UserDTO>> GetUsers();
        Task<List<UserRoleDTO>> GetUserRolesAsync();
        Task<List<UserRoleDTO>> GetRolesByUserIDAsync(int userId);
        Task<List<UserReportRoleDTO>> GetReportRolesByUserIdAsync(int userId);
    }
}
