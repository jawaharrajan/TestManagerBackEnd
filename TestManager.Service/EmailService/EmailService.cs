using TestManager.Domain.DTO.Email;
using TestManager.Service.Helper;
using System.Net.Mail;

namespace TestManager.Service.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailDTO emailDTO);
    }
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(EmailDTO emailDTO)
        {
            return await SendEmailToClient.SendEmail(emailDTO);
        }
    }
}
