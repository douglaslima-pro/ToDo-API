using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities.Tasks;
using ToDo.Domain.Common.Notification;
using ToDo.Application.Abstractions.Validators;

namespace ToDo.Application.Features.Tasks.Validators
{
    public class TaskListValidator : IValidator<TaskList>
    {
        private readonly DomainNotification _domainNotification;

        public TaskListValidator(DomainNotification domainNotification)
        {
            this._domainNotification = domainNotification;
        }

        public bool Validate(TaskList taskList)
        {
            if (string.IsNullOrWhiteSpace(taskList.Title))
            {
                _domainNotification.AddError("Title", "Title is required");
            }

            if (taskList.Title.Length < 5)
            {
                _domainNotification.AddError("Title", "Title must be at least 5 characters long");
            }

            if (taskList.Title.Length > 100)
            {
                _domainNotification.AddError("Title", "Title must be at most 100 characters long");
            }

            return !_domainNotification.HasErrors();
        }
    }
}
