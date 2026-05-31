using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Claims;
using ToDo.API.Models.TaskList;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;

namespace ToDo.API.Controllers
{
    [Authorize]
    [Route("api/taskLists")]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskListService _taskListService;

        public TaskListController(ITaskListService taskListService)
        {
            _taskListService = taskListService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskListModel model)
        {
            var user = User.ExtractClaims();

            if (user == null)
            {
                return Unauthorized();
            }

            var taskList = new CreateTaskListDTO
            {
                Title = model.Title ?? string.Empty,
                UserId = user.Id,
            };

            await _taskListService.CreateAsync(taskList);

            return Created();
        }

        [HttpPut("{taskListId:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int taskListId, [FromBody] UpdateTaskListModel model)
        {
            var taskList = new UpdateTaskListDTO
            {
                TaskListId = taskListId,
                Title = model.Title ?? string.Empty,
            };

            var exists = await _taskListService.ExistsAsync(taskList.TaskListId);

            if (!exists)
            {
                return NotFound();
            }

            await _taskListService.UpdateAsync(taskList);

            return Ok();
        }

        [HttpDelete("{taskListId:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int taskListId)
        {
            var exists = await _taskListService.ExistsAsync(taskListId);

            if (!exists)
            {
                return NotFound();
            }

            await _taskListService.DeleteAsync(taskListId);

            return NoContent();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int start = 0, [FromQuery] int length = 5)
        {
            var user = User.ExtractClaims();

            if (user == null)
            {
                return Unauthorized();
            }

            var taskLists = await _taskListService.GetAllAsync(user.Id, start, length);

            return Ok(taskLists);
        }
    }
}
