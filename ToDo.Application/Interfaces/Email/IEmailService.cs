using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Interfaces.Email
{
    public interface IEmailService
    {
        public Task SendAsync(string to, string subject, string body, bool isHtml = true);
    }
}
