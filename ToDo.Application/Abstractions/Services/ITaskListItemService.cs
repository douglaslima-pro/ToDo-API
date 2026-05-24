using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Features.Tasks.DTOs;

namespace ToDo.Application.Abstractions.Services
{
    public interface ITaskListItemService
    {
        Task<TaskListItemDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TaskListItemDTO>> GetAllAsync(int taskListId, int start = 0, int length = 5);
        Task CreateAsync(CreateTaskListItemDTO model);
    }
}
