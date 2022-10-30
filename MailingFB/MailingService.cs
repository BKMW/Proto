using MailingFB.Dtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailingFB 
{ 
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailSettings;

        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        #region single mail
        public async Task SendEmailAsync(string mailTo, string subject, string body)
        {


            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));


            var builder = new BodyBuilder();


            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            using var smtp = new SmtpClient();
            //smtp.Connect(_mailSettings.HOST, _mailSettings.PORT, SecureSocketOptions.StartTls); //normal
            smtp.Connect(_mailSettings.HOST, _mailSettings.PORT, SecureSocketOptions.None);


            // smtp.Authenticate(_mailSettings.Email, _mailSettings.PWD); // if (SecureSocketOptions.StartTls)
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        #endregion

        #region send stat
        public async Task SendEmailAsync(string subject, string body)
        {


            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.AddRange(_mailSettings.TO as List<MailboxAddress>);
            email.Cc.AddRange(_mailSettings.CC as List<MailboxAddress>);


            var builder = new BodyBuilder();


            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            using var smtp = new SmtpClient();
            //smtp.Connect(_mailSettings.HOST, _mailSettings.PORT, SecureSocketOptions.StartTls); //normal
            smtp.Connect(_mailSettings.HOST, _mailSettings.PORT, SecureSocketOptions.None);


            // smtp.Authenticate(_mailSettings.Email, _mailSettings.PWD); // if (SecureSocketOptions.StartTls)
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        #endregion

        #region stat and files
        public async Task SendEmailAsync( string subject, string body, IList<IFormFile> attachments = null)
        {
          

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.AddRange(_mailSettings.TO as List<MailboxAddress>);
            email.Cc.AddRange(_mailSettings.TO as List<MailboxAddress>);


            var builder = new BodyBuilder();

            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.HOST, _mailSettings.PORT, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.PWD);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        #endregion
    }
}