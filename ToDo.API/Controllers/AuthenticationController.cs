using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ToDo.API.Email;
using ToDo.API.Models.Authentication;
using ToDo.Application.Common.Enums;
using ToDo.Application.Abstractions.Email.Services;
using ToDo.Application.Abstractions.Email.DTOs;
using ToDo.Application.Abstractions.Identity.Services;
using ToDo.Application.Abstractions.Identity.DTOs;

namespace ToDo.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly EmailTemplateReader _emailTemplateReader;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IEmailService emailService,
            EmailTemplateReader emailTemplateReader)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
            _emailTemplateReader = emailTemplateReader;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestModel body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerRequest = new RegisterRequestDTO
            {
                UserName = body.UserName,
                Email = body.Email,
                Password = body.Password,
                PhoneNumber = body.PhoneNumber,
                FirstName = body.FirstName,
                LastName = body.LastName,
                Bio = body.Bio
            };

            var registerResult = await _authenticationService.RegisterAsync(registerRequest);

            if (!registerResult.Succeeded)
            {
                return BadRequest(new
                {
                    errors = registerResult.Errors
                });
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel body)
        {
            var loginRequest = new LoginRequestDTO
            {
                Email = body.Email ?? string.Empty,
                Password = body.Password ?? string.Empty,
            };

            var loginResult = await _authenticationService.LoginAsync(loginRequest);

            if (!loginResult.Succeeded)
            {
                return Unauthorized(new
                {
                    loginResult.Error
                });
            }

            var accessToken = _authenticationService.GenerateAccessTokenAsync(loginResult.User!);

            return Ok(accessToken);
        }

        [HttpPost("email-confirmation/send")]
        public async Task<IActionResult> SendEmailConfirmationTokenAsync([FromBody] SendEmailConfirmationTokenModel model)
        {
            var exists = await _authenticationService.ExistsByEmailAsync(model.Email!);

            if (!exists)
            {
                return Ok();
            }

            var token = await _authenticationService.GenerateEmailConfirmationTokenAsync(model.Email!);

            var link = QueryHelpers.AddQueryString("https://site.com/email-confirmation", new Dictionary<string, string?>
            {
                { "email", model.Email },
                { "token", token }
            });

            var emailTemplateValues = new Dictionary<string, object>
            {
                { "name", model.Name! },
                { "link", link }
            };

            var emailTemplate = await _emailTemplateReader.ReadTemplateAsync("EmailConfirmationTemplate", emailTemplateValues);

            var emailMessage = new EmailMessageDTO
            {
                To = model.Email!,
                Subject = "E-mail confirmation",
                Body = emailTemplate
            };

            await _emailService.SendAsync(emailMessage);

            return Ok();
        }

        [HttpPost("email-confirmation/verify", Name = "ConfirmEmail")]
        public async Task<IActionResult> VerifyEmailConfirmationTokenAsync([FromBody] VerifyEmailConfirmationTokenModel model)
        {
            var confirmEmailRequest = new ConfirmEmailRequestDTO
            {
                Email = model.Email ?? string.Empty,
                Token = model.Token ?? string.Empty,
            };

            var result = await _authenticationService.ConfirmEmailAsync(confirmEmailRequest);

            return new JsonResult(result);
        }

        [HttpPost("password-reset/send")]
        public async Task<IActionResult> SendPasswordResetTokenAsync([FromBody] SendPasswordResetTokenModel model)
        {
            var exists = await _authenticationService.ExistsByEmailAsync(model.Email!);

            if (!exists)
            {
                return Ok();
            }

            var token = await _authenticationService.GeneratePasswordResetTokenAsync(model.Email!);
            
            var link = QueryHelpers.AddQueryString("https://site.com/password-reset", new Dictionary<string, string?>
            {
                { "email", model.Email },
                { "token", token }
            });

            var emailTemplate = await _emailTemplateReader.ReadTemplateAsync("ResetPasswordTemplate", new Dictionary<string, object>
            {
                { "name", model.Name! },
                { "link", link }
            });

            var emailMessage = new EmailMessageDTO
            {
                To = model.Email!,
                Subject = "Reset password",
                Body = emailTemplate
            };

            await _emailService.SendAsync(emailMessage);

            return Ok();
        }

        [HttpPost("password-reset/verify", Name = "ResetPassword")]
        public async Task<IActionResult> VerifyPasswordResetTokenAsync([FromBody] VerifyPasswordResetTokenModel model)
        {
            var resetPasswordRequest = new ResetPasswordRequestDTO
            {
                Email = model.Email ?? string.Empty,
                Token = model.Token ?? string.Empty,
                Password = model.Password ?? string.Empty,
            };

            var result = await _authenticationService.ResetPasswordAsync(resetPasswordRequest);

            return new JsonResult(result);
        }
    }
}
