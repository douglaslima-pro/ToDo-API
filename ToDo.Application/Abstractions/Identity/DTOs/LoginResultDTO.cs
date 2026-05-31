using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Abstractions.Identity.Enums;

namespace ToDo.Application.Abstractions.Identity.DTOs
{
    public class LoginResultDTO
    {
        public bool Succeeded { get; set; }
        public LoginErrorDTO? Error { get; set; }
        public UserDTO? User { get; set; }
    }

    public class LoginErrorDTO
    {
        public LoginErrorTypes Code { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
