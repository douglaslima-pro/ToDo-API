using System.Linq.Expressions;
using System.Reflection;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;
using ToDo.Application.Features.Tasks.Validators;
using ToDo.Domain.Entities.Tasks;
using ToDo.Domain.Repositories;

namespace ToDo.Application.Features.Tasks.Services
{
    public class TaskListService : ITaskListService
    {
        // repositories
        private readonly ITaskListRepository _taskListRepository;

        // validators
        private readonly TaskListValidator _taskListValidator;

        public TaskListService(
            ITaskListRepository taskListRepository,
            TaskListValidator taskListValidator)
        {
            _taskListRepository = taskListRepository;
            _taskListValidator = taskListValidator;
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

            if (!_taskListValidator.Validate(taskList))
            {
                return;
            }

            _taskListRepository.Create(taskList);

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateTaskListDTO model)
        {
            var taskList = await _taskListRepository.GetByIdAsync(model.Id);

            if (taskList == null)
            {
                return false;
            }

            taskList.Rename(model.Title);

            if (!_taskListValidator.Validate(taskList))
            {
                return false;
            }

            _taskListRepository.Update(taskList);

            await _taskListRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var taskList = await _taskListRepository.GetByIdAsync(id);

            if (taskList == null)
            {
                return false;
            }

            _taskListRepository.Delete(taskList);

            await _taskListRepository.SaveChangesAsync();

            return true;
        }
    }
}
