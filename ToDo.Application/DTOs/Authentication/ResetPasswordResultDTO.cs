using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.DTOs.Authentication
{
    public class ResetPasswordResultDTO
    {
        public bool Succeeded { get; set; }
        public IDictionary<string, IEnumerable<string>>? Errors { get; set; }
    }
}
