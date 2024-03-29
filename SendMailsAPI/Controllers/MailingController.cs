using Microsoft.AspNetCore.Mvc;
using SendMailsAPI.DTO;
using SendMailsAPI.Services;

namespace SendMailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailingController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailingController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmailAsync([FromForm] EmailDTO Model)
        {
            try
            {
                await _mailService.SendEmailAsync(Model.Email, Model.Subject, Model.HtmlMessage, Model.Attachements);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
