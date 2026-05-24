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
    }
}
