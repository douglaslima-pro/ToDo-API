using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Abstractions.Validators;
using ToDo.Domain.Common.Notification;
using ToDo.Domain.Entities.TaskLists;

namespace ToDo.Application.Features.Tasks.Validators
{
    public class TaskListItemValidator : IValidator<TaskListItem>
    {
        private readonly DomainNotification _domainNotification;

        public TaskListItemValidator(DomainNotification domainNotification)
        {
            this._domainNotification = domainNotification;
        }

        public bool Validate(TaskListItem taskListItem)
        {
            // title
            if (string.IsNullOrWhiteSpace(taskListItem.Title))
            {
                _domainNotification.AddError("Title", "Title is required.");
            }

            if (taskListItem.Title.Length > 100)
            {
                _domainNotification.AddError("Title", "Title must be less than 100 characters.");
            }

            // description
            if (string.IsNullOrWhiteSpace(taskListItem.Description))
            {
                _domainNotification.AddError("Description", "Description is required.");
            }

            if (taskListItem.Description.Length < 5)
            {
                _domainNotification.AddError("Description", "Description must be greater than 5 characters.");
            }

            if (taskListItem.Description.Length > 500)
            {
                _domainNotification.AddError("Description", "Description must be less than 500 characters.");
            }

            // due date
            if (taskListItem.DueDate <= DateTime.UtcNow)
            {
                _domainNotification.AddError("DueDate", "Due date must be in the future.");
            }

            return !_domainNotification.HasErrors();
        }
    }
}
