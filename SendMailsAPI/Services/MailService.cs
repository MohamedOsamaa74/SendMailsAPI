using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendMailsAPI.Helpers;

namespace SendMailsAPI.Services
{
    public class MailService : IMailService
    {
        #region Fields
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        #endregion

        #region SendEmailAsync
        public async Task SendEmailAsync(string email, string subject, string htmlMessage, IList<IFormFile> Attachements = null)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                var builder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };
                if (Attachements != null)
                {
                    byte[] fileBytes;
                    foreach (var file in Attachements)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                await file.CopyToAsync(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                message.Body = builder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    client.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        #endregion
    }
}
