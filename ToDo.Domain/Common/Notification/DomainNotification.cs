using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                return _errors.ToDictionary(kvp => kvp.Key, kvp => (IEnumerable<string>)kvp.Value);
            }
        }

        public DomainNotification()
        {
            _errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string code, string message)
        {
            if (_errors.ContainsKey(code))
            {
                _errors[code].Add(message);
            }
            else
            {
                _errors.Add(code, [ message ]);
            }
        }

        public void AddErrors(IDictionary<string, List<string>> errors)
        {
            foreach (var error in errors)
            {
                foreach (var message in error.Value)
                {
                    AddError(error.Key, message);
                }
            }
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }
    }
}
