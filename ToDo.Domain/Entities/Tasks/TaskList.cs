using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ToDo.Domain.Entities.Tasks
{
    public class TaskList : AggregateRoot
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int UserId { get; private set; }
        public DateTime? CreatedAt { get; private set; }

        // relationships
        private readonly List<TaskListItem> _tasks = [];
        public IReadOnlyCollection<TaskListItem> Tasks => _tasks;

        private TaskList()
        {
            Title = string.Empty; 
        }

        public TaskList(string title, int userId)
        {
            Title = title;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public TaskListItem AddTask(string title, string description, DateTime dueDate)
        {
            var task = new TaskListItem(title, description, dueDate, this);

            _tasks.Add(task);

            return task;
        }

        public TaskListItem? EditTask(int taskId, string title, string description, DateTime dueDate)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);

            if (task == null)
            {
                return default;
            }

            task.Edit(title, description, dueDate);

            return task;
        }

        public void MarkTaskAsCompleted(int taskId, Action<IDictionary<string, List<string>>> validation)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);

            if (task == null)
            {
                return;
            }

            if (task.IsCompleted == true)
            {
                validation(new Dictionary<string, List<string>>
                {
                    { "Task", new List<string> { "Task is already completed" } }
                });

                return;
            }

            task.MarkAsCompleted();
        }

        public void RemoveTask(int taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);

            if (task != null)
            {
                _tasks.Remove(task);
            }
        }

        public void Rename(string newTitle)
        {
            Title = newTitle;
        }

        public TaskListItem? GetTask(int taskId)
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public bool HasTask(int taskId)
        {
            return _tasks.Any(t => t.Id == taskId);
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

            validation(errors);
        }
    }
}
