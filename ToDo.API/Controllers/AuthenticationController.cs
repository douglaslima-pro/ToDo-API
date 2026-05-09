using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Models.Authentication;
using ToDo.Application.DTOs.Authentication;
using ToDo.Application.Enums;
using ToDo.Application.Interfaces.Email;
using ToDo.Application.Interfaces.Identity;

namespace ToDo.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _environment;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IEmailService emailService,
            IWebHostEnvironment environment)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
            _environment = environment;
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
                var errors = registerResult.Errors.GroupBy(e =>
                    e.Code.Contains("UserName") ? "UserName" :
                    e.Code.Contains("Email") ? "Email" :
                    e.Code.Contains("Password") ? "Password" :
                    e.Code
                )
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Description).ToList()
                    );

                return BadRequest(new
                {
                    errors
                });
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginRequest = new LoginRequestDTO
            {
                Email = body.Email ?? string.Empty,
                Password = body.Password ?? string.Empty,
            };

            var loginResult = await _authenticationService.LoginAsync(loginRequest);

            if (!loginResult.Succeeded)
            {
                if (loginResult.Error?.Code == LoginErrorTypes.EmailNotConfirmed)
                {                        
                    await SendEmailConfirmationTokenAsync(loginRequest.Email, loginResult.User?.FirstName ?? string.Empty);
                }

                return Unauthorized(new
                {
                    loginResult.Error
                });
            }

            var accessToken = _authenticationService.GenerateAccessTokenAsync(loginResult.User!);

            return Ok(accessToken);
        }

        [HttpGet("email-confirmation", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string? email, string? token)
        {
            var result = await _authenticationService.ConfirmEmailAsync(email, token);

            return new JsonResult(result);
        }

        private async Task SendEmailConfirmationTokenAsync(string email, string name)
        {
            var token = await _authenticationService.GenerateEmailConfirmationTokenAsync(email);

            var link = Url.Link("ConfirmEmail", new { email, token });

            var path = Path.Combine(_environment.WebRootPath, "email", "templates", "EmailConfirmationTemplate.html");

            if (!System.IO.File.Exists(path))
            {
                return;
            }

            var emailTemplate = await System.IO.File.ReadAllTextAsync(path, Encoding.UTF8);

            if (string.IsNullOrEmpty(emailTemplate))
            {
                return;
            }

            emailTemplate = emailTemplate
                .Replace("{{name}}", name)
                .Replace("{{link}}", link);

            await _emailService.SendAsync(email, "E-mail confirmation", emailTemplate);
        }
    }
}
