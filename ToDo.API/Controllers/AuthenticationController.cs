using Microsoft.AspNetCore.Authorization;
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

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IEmailService emailService)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
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
                    var emailConfirmationToken = await _authenticationService.GenerateEmailConfirmationTokenAsync(loginRequest.Email);
                }

                return Unauthorized(new
                {
                    loginResult.Error
                });
            }

            var accessToken = _authenticationService.GenerateAccessTokenAsync(loginResult.User!);

            return Ok(accessToken);
        }
    }
}
