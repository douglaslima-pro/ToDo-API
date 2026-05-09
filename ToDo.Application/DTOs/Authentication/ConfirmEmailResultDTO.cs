using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.DTOs.Authentication
{
    public class ConfirmEmailResultDTO
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}
