using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Features.Tasks.DTOs
{
    public class UpdateTaskListDTO
    {
        public int TaskListId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
