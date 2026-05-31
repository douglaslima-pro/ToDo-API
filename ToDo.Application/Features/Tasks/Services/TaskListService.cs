using System.Linq.Expressions;
using System.Reflection;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;
using ToDo.Domain.Common.Notification;
using ToDo.Domain.Entities.Tasks;
using ToDo.Domain.Repositories;

namespace ToDo.Application.Features.Tasks.Services
{
    public class TaskListService : ITaskListService
    {
        // repositories
        private readonly ITaskListRepository _taskListRepository;

        // domain notification
        private readonly DomainNotification _domainNotification;

        public TaskListService(
            ITaskListRepository taskListRepository,
            DomainNotification domainNotification)
        {
            _taskListRepository = taskListRepository;
            _domainNotification = domainNotification;
        }

        public async Task<bool> ExistsAsync(int taskListId)
        {
            return await _taskListRepository.ExistsAsync(t => t.Id == taskListId);
        }

        public async Task<IEnumerable<TaskListDTO>> GetAllAsync(int userId, int start = 0, int length = 5)
        {
            var taskLists = await _taskListRepository.GetPagedAsync(
                    searchExpression: t => t.UserId == userId,
                    orderByExpression: t => t.CreatedAt,
                    includeExpression: ["Tasks"],
                    isAscending: false,
                    start: start,
                    length: length
                );

            return taskLists.Select(t => new TaskListDTO
            {
                Id = t.Id,
                Title = t.Title,
                CreatedAt = t.CreatedAt,
                TasksCount = t.Tasks.Count
            });
        }

        public async Task CreateAsync(CreateTaskListDTO model)
        {
            var taskList = new TaskList(model.Title, model.UserId);

            taskList.Validate(e => _domainNotification.AddErrors(e));

            if (_domainNotification.HasErrors())
            {
                return;
            }

            _taskListRepository.Create(taskList);

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateTaskListDTO model)
        {
            var taskList = await _taskListRepository.GetByIdAsync(model.TaskListId);

            if (taskList == null)
            {
                return;
            }

            taskList.Rename(model.Title);

            taskList.Validate(e => _domainNotification.AddErrors(e));

            if (_domainNotification.HasErrors())
            {
                return;
            }

            _taskListRepository.Update(taskList);

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int taskListId)
        {
            var taskList = await _taskListRepository.GetByIdAsync(taskListId);

            if (taskList == null)
            {
                return;
            }

            _taskListRepository.Delete(taskList);

            await _taskListRepository.SaveChangesAsync();
        }
    }
}
