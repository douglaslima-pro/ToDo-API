using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Features.Tasks.DTOs;

namespace ToDo.Application.Abstractions.Services
{
    public interface ITaskListService
    {
        Task<IEnumerable<TaskListDTO>> GetAllAsync(int userId, int start = 0, int length = 5);
        Task CreateAsync(CreateTaskListDTO model);
        Task<bool> UpdateAsync(UpdateTaskListDTO model);
        Task<bool> DeleteAsync(int id);
    }
}
