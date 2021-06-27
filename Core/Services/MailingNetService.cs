using Core.Dtos;
using Core.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Core.Services
{
    public class MailingNetService : IMailingNetService
    {
        private readonly MailSettings _mailSettings;

        public MailingNetService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        #region single mail
        public async Task SendEmailNetAsync(string mailTo, string subject, string body)
        {

          
                var _sender = _mailSettings.Email;
                var _displayName = _mailSettings.DisplayName;
                var _pwd = _mailSettings.PWD;
                var _host = _mailSettings.HOST;
                int _port = _mailSettings.PORT;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_host);
                mail.From = new MailAddress(_sender, _displayName);
                mail.To.Add(mailTo);

                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = _port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_sender, _pwd);
                SmtpServer.EnableSsl = true;
               
                await SmtpServer.SendMailAsync(mail);
               
            
        }
        #endregion

    
    }
}