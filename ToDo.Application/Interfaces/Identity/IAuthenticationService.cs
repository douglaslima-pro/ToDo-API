using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.DTOs.Account;
using ToDo.Application.DTOs.Authentication;

namespace ToDo.Application.Interfaces.Identity
{
    public interface IAuthenticationService
    {
        public Task<RegisterResultDTO> RegisterAsync(RegisterRequestDTO registerRequest);
        public Task<LoginResultDTO> LoginAsync(LoginRequestDTO loginRequest);
        public string GenerateAccessTokenAsync(UserDTO user);
        public Task<string> GenerateEmailConfirmationTokenAsync(string email);
        public Task<ConfirmEmailResultDTO> ConfirmEmailAsync(string? email, string? token);
    }
}
