using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Users;
using TestManager.Interfaces;

namespace TestManager.Service.UserContext
{
    public class UserContextService(IUserRepository userRepository) : IUserContextService
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int TipsUserId { get; set; }

        public void GetUserId()
        {

            UserDTO userDTO = new()
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName
            };

            TipsUserId = userRepository.GetUserId(userDTO);
        }

        public async Task GetUserIdAsync()
        {

            UserDTO userDTO = new()
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName
            };

            TipsUserId = await userRepository.GetUserIdAsync(userDTO);
        }

        public async Task<UserDTO> GetUserDetails(UserDTO userDTO)
        {
            return await userRepository.GetUserDetails(userDTO);
        }
        public async Task<List<UserDTO>> GetUsers()
        {
            return await userRepository.GetUsers();
        }

        public async Task<List<UserRoleDTO>> GetUserRolesAsync()
        {
            return await userRepository.GetUserRolesAsync();
        }

        public async Task<List<UserRoleDTO>> GetRolesByUserIDAsync(int userId)
        {
            return await userRepository.GetRolesByUserIDAsync(userId);
        }

        public async Task<List<UserReportRoleDTO>> GetReportRolesByUserIdAsync(int userId)
        {
            return await userRepository.GetReportRolesByUserIdAsync(userId);
        }
    }
}
