using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Users;
using TestManager.Service.Helper;
using TestManager.Domain.Model.UserManagement;
using TestManager.Service;
using TestManager.Service.Uploader;
using TestManager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Runtime.ExceptionServices;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.User;

public class UserFunction(IUserContextService userContextService, ILogger<UserFunction> logger) : BaseFunction(logger)
{
    [Function("GetUsers")]
    public async Task<OkObjectResult> GetUsers([HttpTrigger(AuthorizationLevel.Function, "get", Route ="user")] HttpRequest req)
    {
        logger.LogInformation("Fetching all Users");        

        var users = await userContextService.GetUsers();

        logger.LogInformation($"Retrieved {users.Count} Users");

        return new OkObjectResult(users);
    }

    [Function("GetUserRoles")]
    public async Task<OkObjectResult> GetUserRoles([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/role")] HttpRequest req)
    {
        logger.LogInformation("Fetching all Users and Roles");

        var users = await userContextService.GetUserRolesAsync();

        logger.LogInformation($"Retrieved {users.Count} Users Roles");

        return new OkObjectResult(users);
    }

    [Function("GetUserRoleByUserId")]
    public async Task<IActionResult> GetUserRoleByUserId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/role/{id}")] HttpRequest req,
        int? id)
    {       
        if (id.HasValue)
        {
            logger.LogInformation($"Fetching all User Roles for UserId : {id}");
            return await ExecuteSafeAsync(
            async () =>
            {
                var userRoles = await userContextService.GetRolesByUserIDAsync(id.Value) ??
                    throw new KeyNotFoundException($"Roles details for Id: {id} Not found");
                return userRoles;
            }, $"Get User Roles detail for userId : {id}");
        }
        else
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: UserID is required.", false));
    }

    [Function("GetUserReportRoleByUserId")]
    public async Task<IActionResult> GetUserReportRoleByUserId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/reportrole/{id}")] HttpRequest req,
    int? id)
    {
        if (id.HasValue)
        {
            logger.LogInformation($"Fetching all User Roles for UserId : {id}");
            return await ExecuteSafeAsync(
            async () =>
            {
                var userRoles = await userContextService.GetReportRolesByUserIdAsync(id.Value) ??
                    throw new KeyNotFoundException($"User Report Roles details for Id: {id} Not found");
                return userRoles;
            }, $"Get User Report Roles detail for userId : {id}");
        }
        else
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid payload: UserID is required.", false));
    }

    [Function("GetUserDetails")]
    public async Task<IActionResult> GetUserDetails([HttpTrigger(AuthorizationLevel.Function, "get", Route = "userdetail")] HttpRequest req)
    {
        UserDTO userDto;

        if (req.QueryString.HasValue) 
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
            userDto = new UserDTO
            {
                Email = string.IsNullOrEmpty(query["email"]) ? string.Empty : query["email"],
                FirstName = string.IsNullOrEmpty(query["firstName"]) ? string.Empty : query["firstName"],
                LastName = string.IsNullOrEmpty(query["lastName"]) ? string.Empty : query["lastName"]
            };

            logger.LogInformation($"Fetching User with Email: {userDto.Email}, FirstName: {userDto.FirstName}, LastNameL {userDto.LastName}");
            return await ExecuteSafeAsync(
            async () =>
            {
                var user = await userContextService.GetUserDetails(userDto) ??
                    throw new KeyNotFoundException($"User with Email: {userDto.Email} Not found");
                return user;
            }, $"Get User Details");
        }
        else
            return new BadRequestObjectResult(
                new ApiResponse<string>("Invalid Request: Must send Email,FistName and LastNanme.", false));      
    }
}