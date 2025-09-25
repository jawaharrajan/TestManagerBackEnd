using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Users;
using CMSUser = TestManager.Domain.Model.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Users
{
    public class UserRepository(ApplicationDbContext _) : GenericRepository<CMSUser.User, int>(_), IUserRepository
    {
        public int GetUserId(UserDTO userDTO)
        {
            var user = from u in _context.User
                         where u.Email == userDTO.Email && u.FirstName == userDTO.FirstName && u.LastName == userDTO.LastName
                         select u.UserId;

            if (user.Any())
            {
                return user.First();
            }
            return -1;
            
        }

        public async Task<int> GetUserIdAsync(UserDTO userDTO)
        {
            var user = from u in _context.User
                       where u.Email == userDTO.Email && u.FirstName == userDTO.FirstName && u.LastName == userDTO.LastName
                       select u.UserId;

            if (user.Any())
            {
                return await user.FirstAsync();
            }
            return -1;
        }

        public async Task<UserDTO> GetUserDetails(UserDTO userDTO)
        {
            UserDTO? user = await (from u in _context.User
                              where u.Email == userDTO.Email &&
                                    u.FirstName == userDTO.FirstName &&
                                    u.LastName == userDTO.LastName
                               select new UserDTO
                               {
                                   UserId = u.UserId,
                                   Username = u.UserName,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Email = u.Email,
                                   Phonenumber = $"{u.DirectAreaCode}-{u.DirectPhone}",
                                   Credentials = u.Credentials,
                                   Title = u.JobTitle
                               }).FirstOrDefaultAsync();

            return user;
        }
        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await (from u in _context.User                      
                       select new UserDTO
                       {
                           UserId = u.UserId, 
                           Username = u.UserName,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Email = u.Email,
                           Phonenumber = $"{u.DirectAreaCode}-{u.DirectPhone}",
                           Credentials = u.Credentials,
                           Title = u.JobTitle
                       }).ToListAsync();

            return users;
        }

        public async Task<List<UserRoleDTO>> GetUserRolesAsync()
        {
            var result = await (from u in _context.User
                               join ur in _context.UserRole
                                   on u.UserId equals ur.UserID
                               join r in _context.Role
                                   on ur.RoleID equals r.RoleId
                               select new UserRoleDTO
                               {
                                   UserID = u.UserId,
                                   RoleID = ur.RoleID,
                                   RoleName = r.Name
                               }).ToListAsync();

            return result;
        }

        public async Task<List<UserRoleDTO>> GetRolesByUserIDAsync(int userId)
        {
            var result = await (
                from u in _context.User
                join ur in _context.UserRole on u.UserId equals ur.UserID
                join r in _context.Role on ur.RoleID equals r.RoleId
                where ur.UserID == userId
                    select new UserRoleDTO
                    {
                        UserID = u.UserId,
                        RoleID = ur.RoleID,
                        RoleName = r.Name
                    }
                 ).ToListAsync();

            return result;
        }

        public async Task<List<UserReportRoleDTO>> GetReportRolesByUserIdAsync(int userId)
        {
            var reportRoleIds = await _context.RoleReport
            .Select(rr => rr.RoleID)
            .Distinct()
            .ToListAsync();

            var result = await (
                from u in _context.User
                join ur in _context.UserRole on u.UserId equals ur.UserID
                join r in _context.Role on ur.RoleID equals r.RoleId
                where reportRoleIds.Contains(r.RoleId)
                join rr in _context.RoleReport on r.RoleId equals rr.RoleID
                where ur.UserID == userId
                    select new UserReportRoleDTO
                    {
                        UserID = u.UserId,
                        RoleID = ur.RoleID,
                        RoleName = r.Name,
                        Report = rr.Report
                    }
                ).ToListAsync();

            return result;
        }
    }
}
