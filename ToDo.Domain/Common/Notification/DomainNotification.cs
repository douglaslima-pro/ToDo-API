using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain.Common.Notification
{
    public class DomainNotification
    {
        private readonly IDictionary<string, List<string>> _errors;

        public IReadOnlyDictionary<string, IEnumerable<string>> Errors
        {
            get
            {
                return (IReadOnlyDictionary<string, IEnumerable<string>>)_errors;
            }
        }

        public DomainNotification()
        {
            _errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string errorCode, string message)
        {
            if (_errors.ContainsKey(errorCode))
            {
                _errors[errorCode].Add(message);
            }
            else
            {
                _errors.Add(errorCode, new List<string> { message });
            }
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }
    }
}
