using TestManager.Domain.DTO.Email;
using System.Net.Mail;

namespace TestManager.Service.Helper
{
    public static class SendEmailToClient
    {
        public static Task<bool> SendEmail(EmailDTO emailDTO)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;

            string mailUrl = "testclient-com.mail.protection.outlook.com";

            mail.From = new MailAddress(emailDTO.FromEmail, emailDTO.FromName);
            mail.To.Add(emailDTO.To);            // put to address here
            mail.Subject = emailDTO.Subject.Trim();        // put subject here 
            mail.Body = emailDTO.Body;           // put body of email here

            SmtpClient smpt = new(mailUrl);

            // and then send the mail
            try
            {
                smpt.UseDefaultCredentials = true;
                smpt.EnableSsl = true;
                smpt.Send(mail);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        
        }
    }    
}
