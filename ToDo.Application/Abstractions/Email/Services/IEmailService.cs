using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Abstractions.Email.DTOs;

namespace ToDo.Application.Abstractions.Email.Services
{
    public interface IEmailService
    {
        public Task SendAsync(EmailMessageDTO emailMessage);
    }
}
