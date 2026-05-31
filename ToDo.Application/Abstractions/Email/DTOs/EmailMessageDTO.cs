using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Abstractions.Email.DTOs
{
    public class EmailMessageDTO
    {
        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public bool IsHtml { get; set; } = true;
    }
}
