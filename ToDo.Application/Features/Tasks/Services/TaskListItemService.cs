using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;
using ToDo.Application.Features.Tasks.Validators;
using ToDo.Domain.Repositories;

namespace ToDo.Application.Features.Tasks.Services
{
    public class TaskListItemService : ITaskListItemService
    {
        // repositories
        private readonly ITaskListRepository _taskListRepository;

        // validators
        private readonly TaskListItemValidator _taskListItemValidator;

        public TaskListItemService(
            ITaskListRepository taskListRepository,
            TaskListItemValidator taskListItemValidator)
        {
            _taskListRepository = taskListRepository;
            _taskListItemValidator = taskListItemValidator;
        }

        public async Task<TaskListItemDTO?> GetByIdAsync(int id)
        {
            var task = await _taskListRepository.GetTaskByIdAsync(id);
            
            if (task == null)
            {
                return null;
            }
            
            return new TaskListItemDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
            };
        }

        public async Task<IEnumerable<TaskListItemDTO>> GetAllAsync(int taskListId, int start = 0, int length = 5)
        {
            var tasks = await _taskListRepository.GetAllTasksFromListAsync(
                    taskListId,
                    orderByExpression: (t) => t.CreatedAt,
                    isAscending: false,
                    start: start,
                    length: length
                );

            return tasks.Select(t => new TaskListItemDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate,
            });
        }

        public async Task CreateAsync(CreateTaskListItemDTO model)
        {
            var taskList = await _taskListRepository.GetByIdAsync(model.TaskListId);

            if (taskList == null)
            {
                return;
            }

            var taskListItem = taskList.AddTask(model.Title, model.Description, model.DueDate);

            if (!_taskListItemValidator.Validate(taskListItem))
            {
                return;
            }

            _taskListRepository.Update(taskList);

            await _taskListRepository.SaveChangesAsync();
        }
    }
}
