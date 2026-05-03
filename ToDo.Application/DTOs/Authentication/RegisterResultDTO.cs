using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.DTOs.Authentication
{
    public class RegisterResultDTO
    {
        public bool Succeeded { get; set; }
        public IEnumerable<RegisterErrorDTO> Errors { get; set; } = [];
    }

    public class RegisterErrorDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
