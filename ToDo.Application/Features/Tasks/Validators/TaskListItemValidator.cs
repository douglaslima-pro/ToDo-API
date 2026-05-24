using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Abstractions.Validators;
using ToDo.Domain.Common.Notification;
using ToDo.Domain.Entities.Tasks;

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
                _domainNotification.AddError("Title", "Title is required");
            }

            if (taskListItem.Title.Length < 5)
            {
                _domainNotification.AddError("Title", "Title must be at least 5 characters long");
            }

            if (taskListItem.Title.Length > 100)
            {
                _domainNotification.AddError("Title", "Title must be at most 100 characters long");
            }

            // description
            if (string.IsNullOrWhiteSpace(taskListItem.Description))
            {
                _domainNotification.AddError("Description", "Description is required");
            }

            if (taskListItem.Description.Length < 5)
            {
                _domainNotification.AddError("Description", "Description must be at least 5 characters long");
            }

            if (taskListItem.Description.Length > 500)
            {
                _domainNotification.AddError("Description", "Description must be at most 500 characters long");
            }

            // due date
            if (taskListItem.DueDate <= DateTime.UtcNow)
            {
                _domainNotification.AddError("DueDate", "Due date must be in the future");
            }

            return !_domainNotification.HasErrors();
        }
    }
}
