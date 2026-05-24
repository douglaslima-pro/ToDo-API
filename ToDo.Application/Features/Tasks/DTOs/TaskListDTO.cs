using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Features.Tasks.DTOs
{
    public class TaskListDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TasksCount { get; set; }
    }
}
