using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDo.Application.DTOs.Account;
using ToDo.Application.DTOs.Authentication;
using ToDo.Application.Enums;
using ToDo.Application.Interfaces.Identity;
using ToDo.Infrastructure.Data.Identity.Entities;

namespace ToDo.Infrastructure.Data.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ToDoUser> _signInManager;
        private readonly UserManager<ToDoUser> _userManager;

        // JWT Settings
        private readonly string _key;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly int _expiration;

        public AuthenticationService(
            SignInManager<ToDoUser> signInManager,
            UserManager<ToDoUser> userManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;

                _key = configuration.GetValue<string>("Jwt:Key")!;
                _audience = configuration.GetValue<string>("Jwt:Audience")!;
                _issuer = configuration.GetValue<string>("Jwt:Issuer")!;
                _expiration = configuration.GetValue<int>("Jwt:Expiration");
        }

        public async Task<RegisterResultDTO> RegisterAsync(RegisterRequestDTO registerRequest)
        {
            var user = new ToDoUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Bio = registerRequest.Bio,
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            return new RegisterResultDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Succeeded ? [] : result.Errors.Select(e => new RegisterErrorDTO
                {
                    Code = e.Code,
                    Description = e.Description,
                })
            };
        }

        public async Task<LoginResultDTO> LoginAsync(LoginRequestDTO loginRequest)
        {
            var loginResult = new LoginResultDTO();

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                loginResult.Succeeded = false;

                loginResult.Error = new LoginErrorDTO
                {
                    Code = LoginErrorTypes.InvalidLoginAttempt,
                    Description = "Invalid login attempt"
                };

                return loginResult;
            }

            loginResult.Succeeded = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (!loginResult.Succeeded)
            {
                loginResult.Error = new LoginErrorDTO
                {
                    Code = LoginErrorTypes.InvalidLoginAttempt,
                    Description = "Invalid login attempt"
                };

                return loginResult;
            }

            loginResult.User = new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
            };

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isEmailConfirmed)
            {
                loginResult.Succeeded = false;

                loginResult.Error = new LoginErrorDTO
                {
                    Code = LoginErrorTypes.EmailNotConfirmed,
                    Description = "Email is not confirmed"
                };

                return loginResult;
            }

            return loginResult;
        }

        public string GenerateAccessTokenAsync(UserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _audience,
                Issuer = _issuer,
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = signingCredentials,
                Expires = DateTime.UtcNow.AddMinutes(_expiration),
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return string.Empty;
            }

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }
    }
}
