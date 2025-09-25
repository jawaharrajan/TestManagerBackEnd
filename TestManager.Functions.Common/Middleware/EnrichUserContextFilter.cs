using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.IdentityModel.Tokens.Jwt;
using Serilog.Context;
using Microsoft.Extensions.DependencyInjection;
using TestManager.Interfaces;

public class EnrichUserContextMiddleware() : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // Create a scope from the FunctionContext
        var serviceProvider = context.InstanceServices;
        var userContextService = serviceProvider.GetService<IUserContextService>();

        // Extract HTTP request
        var httpReq = await context.GetHttpRequestDataAsync();
        if (httpReq != null && httpReq.Headers.TryGetValues("Authorization", out var authHeaderValues))
        {
            var tokenValue = authHeaderValues.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(tokenValue) && tokenValue.StartsWith("Bearer "))
            {
                var jwt = tokenValue.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(jwt) as JwtSecurityToken;

                userContextService.Email = token?.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                userContextService.FirstName = token?.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
                userContextService.LastName = token?.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;

                await userContextService.GetUserIdAsync();

                //set UserID for Logging 
                LogContext.PushProperty("UserId", userContextService.Email);
            }
        }

        // Call the next middleware or function
        await next(context);
    }
}

