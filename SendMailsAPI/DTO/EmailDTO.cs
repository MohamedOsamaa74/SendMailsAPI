using System.ComponentModel.DataAnnotations;

namespace SendMailsAPI.DTO
{
    public class EmailDTO
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Subject { get; set; }
        [Required]
        public required string HtmlMessage { get; set; }
        public IList<IFormFile>? Attachements { get; set; }
    }
}
