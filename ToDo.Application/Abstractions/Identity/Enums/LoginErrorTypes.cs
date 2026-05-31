using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Abstractions.Identity.Enums
{
    public enum LoginErrorTypes
    {
        InvalidLoginAttempt = 0,
        EmailNotConfirmed = 1
    }
}
