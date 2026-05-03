using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.DTOs.Account;

namespace ToDo.Application.DTOs.Authentication
{
    public class LoginResultDTO
    {
        public bool Succeeded { get; set; }
        public LoginErrorDTO? Error { get; set; }
        public UserDTO? User { get; set; }
    }

    public class LoginErrorDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
