using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain.Entities.TaskLists
{
    public class TaskListItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime DueDate { get; private set; }
        public int TaskListId { get; private set; }

        // relationships
        public TaskList? TaskList { get; private set; }

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
    }
}
