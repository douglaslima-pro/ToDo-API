using System.Linq.Expressions;
using System.Reflection;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;
using ToDo.Application.Features.Tasks.Validators;
using ToDo.Domain.Entities.TaskLists;
using ToDo.Domain.Repositories;

namespace ToDo.Application.Features.Tasks.Services
{
    public class TaskListService : ITaskListService
    {
        // repositories
        private readonly ITaskListRepository _taskListRepository;

        // validators
        private readonly TaskListValidator _taskListValidator;
        private readonly TaskListItemValidator _taskListItemValidator;

        public TaskListService(
            ITaskListRepository taskListRepository,
            TaskListValidator taskListValidator,
            TaskListItemValidator taskListItemValidator)
        {
            _taskListRepository = taskListRepository;
            _taskListValidator = taskListValidator;
            _taskListItemValidator = taskListItemValidator;
        }

        public async Task<IEnumerable<TaskListDTO>> GetAllAsync(int userId, int start = 0, int length = 5)
        {
            var taskLists = await _taskListRepository.GetPagedAsync(
                    searchExpression: (t) => t.UserId == userId,
                    orderByExpression: (t) => t.CreatedAt,
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

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task EditAsync(EditTaskListDTO model)
        {
            var taskList = await _taskListRepository.GetByIdAsync(model.Id);

            if (taskList == null)
            {
                return;
            }

            taskList.Rename(model.Title);

            if (!_taskListValidator.Validate(taskList))
            {
                return;
            }

            await _taskListRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taskList = await _taskListRepository.GetByIdAsync(id);

            if (taskList == null)
            {
                return;
            }

            _taskListRepository.Delete(taskList);
            await _taskListRepository.SaveChangesAsync();
        }
    }
}
