using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Abstractions.Identity.DTOs;

namespace ToDo.Application.Abstractions.Identity.Services
{
    public interface IAuthenticationService
    {
        public Task<RegisterResultDTO> RegisterAsync(RegisterRequestDTO registerRequest);
        public Task<LoginResultDTO> LoginAsync(LoginRequestDTO loginRequest);
        public string GenerateAccessTokenAsync(UserDTO user);
        public Task<string> GenerateEmailConfirmationTokenAsync(string email);
        public Task<ConfirmEmailResultDTO> ConfirmEmailAsync(ConfirmEmailRequestDTO confirmEmailRequest);
        public Task<string> GeneratePasswordResetTokenAsync(string email);
        public Task<ResetPasswordResultDTO> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordRequest);
        public Task<bool> ExistsByEmailAsync(string email);
    }
}
