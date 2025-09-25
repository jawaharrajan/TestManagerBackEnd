using TestManager.Domain.DTO.Email;
using TestManagerBackEnd.Functions.Uploader;
using TestManager.Service.Helper;
using TestManager.Domain.Model;
using TestManager.Service;
using TestManager.Service.EmailService;
using TestManager.Service.Uploader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using TestManager.Functions.Common;

namespace TestManagerBackEnd.Functions.Email;

public class EmailServiceFunction(IEmailService emailService, ILogger<PatientFunction> logger) : BaseFunction(logger)
{

    [Function("EmailServiceFunction")]
    public async Task<IActionResult> SendEmail([HttpTrigger(AuthorizationLevel.Function, "post", Route = "email/send" )] HttpRequest req)
    {
        EmailDTO emailDTO = await req.ReadFromJsonAsync<EmailDTO>();

        if (emailDTO is null || string.IsNullOrEmpty(emailDTO.To) || string.IsNullOrEmpty(emailDTO.Body)
            || string.IsNullOrEmpty(emailDTO.Subject))
        {
            return new BadRequestObjectResult(
                     new ApiResponse<string>("Invalid payload: Email cannot be null and/or Email details missing", false));
        }
        return await ExecuteSafeAsync(async () =>
        {
            bool result = await emailService.SendEmail(emailDTO);
            return result;
        }, "Send Email Success");      
    }
}