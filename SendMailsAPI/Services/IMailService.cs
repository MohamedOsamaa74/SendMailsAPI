namespace SendMailsAPI.Services
{
    public interface IMailService
    {
        public Task SendEmailAsync(string email, string subject
            , string htmlMessage, IList<IFormFile> Attachements = null);
    }
}