using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Abstractions.Identity.DTOs
{
    public class ResetPasswordResultDTO
    {
        public bool Succeeded { get; set; }
        public IDictionary<string, IEnumerable<string>>? Errors { get; set; }
    }
}
