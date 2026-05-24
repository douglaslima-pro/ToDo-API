using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Abstractions.Validators
{
    public interface IValidator<T>
        where T : class
    {
        bool Validate(T t);
    }
}
