using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.Bal.Services;
using App.Entity.Models.Mail;
using App.Foundation.Common;

namespace VeridocApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly ICaptchaService _captchaService;

        public MailController(IMailService mailService, ICaptchaService captchaService)
        {
            _mailService = mailService;
            _captchaService = captchaService;
        }

        [HttpPost("send-contact-mail")]
        public async Task<IActionResult> SendContactMail(ContactMail contactMail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    App.Entity.Models.HttpResponse httpResponse = await _captchaService.VerifyCaptcha(contactMail.RecaptchaToken);
                    if (!httpResponse.IsSuccess)
                    {
                        return StatusCode(StatusCodes.Status417ExpectationFailed, httpResponse.Content);
                    }
                    _mailService.SendContactMail(contactMail);
                    return Ok();
                }
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages._500ServerError);
            }
        }


        [HttpPost("send-demo-request")]
        public IActionResult SendDemoRequestMail(DemoRequestMail requestMail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _mailService.SendDemoMail(requestMail);
                    return Ok();
                }
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages._500ServerError);
            }
        }


        [HttpPost("send-custom-plan")]
        public IActionResult SendCustomPlanQuote([FromBody]CustomPlan customPlan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     _mailService.SendCustomPlanQuote(customPlan);
                    return Ok(customPlan);
                }
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList());
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages._500ServerError);
            }
        }
    }
}
