using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.DTOs.Authentication
{
    public class RegisterRequestDTO
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
    }
}
