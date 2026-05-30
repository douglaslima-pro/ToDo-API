using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain.Entities.Tasks
{
    public class TaskListItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool? IsCompleted { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime DueDate { get; private set; }
        public int TaskListId { get; private set; }

        // relationships
        public TaskList? TaskList { get; private set; }

        private TaskListItem()
        {
            Title = string.Empty;
            Description = string.Empty;
        }

        public TaskListItem(string title, string description, DateTime dueDate, TaskList taskList)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            TaskListId = taskList.Id;
            IsCompleted = false;
            CreatedAt = DateTime.UtcNow;
            TaskList = taskList;
        }

        public void Edit(string title, string description, DateTime dueDate)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public void Validate(Action<IDictionary<string, List<string>>> validation)
        {
            IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

            void AddError(string code, string message)
            {
                if (errors.ContainsKey(code))
                {
                    errors[code].Add(message);
                }
                else
                {
                    errors.Add(code, [message]);
                }
            }

            // title
            if (string.IsNullOrWhiteSpace(Title))
            {
                AddError("Title", "Title is required");
            }

            if (Title.Length < 5)
            {
                AddError("Title", "Title must be at least 5 characters long");
            }

            if (Title.Length > 100)
            {
                AddError("Title", "Title must be at most 100 characters long");
            }

            // description
            if (string.IsNullOrWhiteSpace(this.Description))
            {
                AddError("Description", "Description is required");
            }

            if (Description.Length < 5)
            {
                AddError("Description", "Description must be at least 5 characters long");
            }

            if (Description.Length > 500)
            {
                AddError("Description", "Description must be at most 500 characters long");
            }

            // due date
            if (DueDate <= DateTime.UtcNow)
            {
                AddError("DueDate", "Due date must be in the future");
            }

            validation(errors);
        }
    }
}
