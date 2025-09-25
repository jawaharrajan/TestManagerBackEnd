using TestManager.Domain.DTO.Users;
using TestManager.Domain.Model.UserManagement;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        int GetUserId(UserDTO userDTO);
        Task<int> GetUserIdAsync(UserDTO userDTO);

        Task<UserDTO> GetUserDetails(UserDTO userDTO);
        Task<List<UserDTO>> GetUsers();

        Task<List<UserRoleDTO>> GetUserRolesAsync();

        Task<List<UserRoleDTO>> GetRolesByUserIDAsync(int userId);

        Task<List<UserReportRoleDTO>> GetReportRolesByUserIdAsync(int userId);
    }
}
