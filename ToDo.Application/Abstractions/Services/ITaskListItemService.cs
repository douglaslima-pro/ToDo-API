using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Features.Tasks.DTOs;

namespace ToDo.Application.Abstractions.Services
{
    // TODO
    // 1. criar tarefa (feito)
    // 2. listar tarefas (feito)
    // 3. visualizar tarefa (feito)
    // 4. editar titulo, nome e data de vencimento (feito)
    // 5. marcar como concluída (feito)
    // 6. excluir tarefa
    public interface ITaskListItemService
    {
        Task<bool> ExistsAsync(int taskId, int taskListId);
        Task<TaskListItemDTO?> GetByIdAsync(int taskId, int taskListId);
        Task<IEnumerable<TaskListItemDTO>> GetAllAsync(int taskListId, int start = 0, int length = 5);
        Task CreateAsync(CreateTaskListItemDTO model);
        Task UpdateAsync(UpdateTaskListItemDTO model);
        Task MarkAsCompletedAsync(int taskId, int taskListId);
        Task DeleteAsync(int taskId, int taskListId);
    }
}
