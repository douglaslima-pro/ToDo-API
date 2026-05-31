using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;
using ToDo.Domain.Common.Notification;
using ToDo.Domain.Repositories;

namespace ToDo.Application.Features.Tasks.Services
{
    public class TaskListItemService : ITaskListItemService
    {
        // repositories
        private readonly ITaskListRepository _taskListRepository;

        // domain notification
        private readonly DomainNotification _domainNotification;

        public TaskListItemService(
            ITaskListRepository taskListRepository,
            DomainNotification domainNotification)
        {
            _taskListRepository = taskListRepository;
            _domainNotification = domainNotification;
        }

        public async Task<bool> ExistsAsync(int taskId, int taskListId)
        {
            var taskList = await _taskListRepository.GetByIdAsync(taskListId);

            if (taskList == null)
            {
                return false;
            }

            await _taskListRepository.LoadAsync(taskList, t => t.Tasks);

            return taskList.HasTask(taskId);
        }

        public async Task<TaskListItemDTO?> GetByIdAsync(int taskId, int taskListId)
        {
            var taskList = await _taskListRepository.GetByIdAsync(taskListId);

            if (taskList == null)
            {
                return default;
            }

            await _taskListRepository.LoadAsync(taskList, t => t.Tasks);

            var task = taskList.GetTask(taskId);

            if (task == null)
            {
                return default;
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

            taskList
                .AddTask(model.Title, model.Description, model.DueDate)
                .Validate(e => _domainNotification.AddErrors(e));

            if (_domainNotification.HasErrors())
            {
                return;
            }

            _taskListRepository.Update(taskList);

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateTaskListItemDTO model)
        {
            var taskList = await _taskListRepository.GetByIdAsync(model.TaskListId);

            if (taskList == null)
            {
                return;
            }

            await _taskListRepository.LoadAsync(taskList, t => t.Tasks);

            var taskListItem = taskList.EditTask(model.TaskId, model.Title, model.Description, model.DueDate);

            if (taskListItem == null)
            {
                return;
            }

            taskListItem.Validate(e => _domainNotification.AddErrors(e));

            if (_domainNotification.HasErrors())
            {
                return;
            }

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task MarkAsCompletedAsync(int taskId, int taskListId)
        {
            var taskList = await _taskListRepository.GetByIdAsync(taskListId);

            if (taskList == null)
            {
                return;
            }

            await _taskListRepository.LoadAsync(taskList, t => t.Tasks);

            taskList.MarkTaskAsCompleted(taskId, e => _domainNotification.AddErrors(e));

            if (_domainNotification.HasErrors())
            {
                return;
            }

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int taskId, int taskListId)
        {
            var taskList = await _taskListRepository.GetByIdAsync(taskListId);
            
            if (taskList == null)
            {
                return;
            }
            
            await _taskListRepository.LoadAsync(taskList, t => t.Tasks);
            
            taskList.RemoveTask(taskId);

            await _taskListRepository.SaveChangesAsync();
        }
    }
}
