using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Claims;
using ToDo.API.Models.TaskListItem;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;
using ToDo.Domain.Entities.Tasks;

namespace ToDo.API.Controllers
{
    [Authorize]
    [Route("api/taskLists/{taskListId:int}/tasks")]
    [ApiController]
    public class TaskListItemController : ControllerBase
    {
        private readonly ITaskListItemService _taskListItemService;

        public TaskListItemController(ITaskListItemService taskListItemService)
        {
            _taskListItemService = taskListItemService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskListItemModel model, [FromRoute] int taskListId)
        {
            var createTaskListItem = new CreateTaskListItemDTO
            {
                Title = model.Title ?? string.Empty,
                Description = model.Description ?? string.Empty,
                DueDate = model.DueDate,
                TaskListId = taskListId,
            };

            await _taskListItemService.CreateAsync(createTaskListItem);

            return Created();
        }

        [HttpPut("{taskId:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int taskId, [FromRoute] int taskListId, [FromBody] UpdateTaskListItemModel model)
        {
            var exists = await _taskListItemService.ExistsAsync(taskId, taskListId);

            if (!exists)
            {
                return NotFound();
            }

            var updateTaskListItem = new UpdateTaskListItemDTO
            {
                TaskId = taskId,
                Title = model.Title ?? string.Empty,
                Description = model.Description ?? string.Empty,
                DueDate = model.DueDate,
                TaskListId = taskListId,
            };

            await _taskListItemService.UpdateAsync(updateTaskListItem);
            
            return Ok();
        }

        [HttpGet("{taskId:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int taskId, [FromRoute] int taskListId)
        {
            var taskListItem = await _taskListItemService.GetByIdAsync(taskId, taskListId);

            return Ok(taskListItem);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromRoute] int taskListId, [FromQuery] int start = 0, [FromQuery] int length = 5)
        {
            var tasks = await _taskListItemService.GetAllAsync(taskListId, start, length);

            return Ok(tasks);
        }

        [HttpPut("markAsComplete/{taskId:int}")]
        public async Task<IActionResult> MarkAsCompletedAsync([FromRoute] int taskId, [FromRoute] int taskListId)
        {
            await _taskListItemService.MarkAsCompletedAsync(taskId, taskListId);

            return Ok();
        }

        [HttpDelete("{taskId:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int taskId, [FromRoute] int taskListId)
        {
            var exists = await _taskListItemService.ExistsAsync(taskId, taskListId);

            if (!exists)
            {
                return NotFound();
            }

            await _taskListItemService.DeleteAsync(taskId, taskListId);

            return NoContent();
        }
    }
}
