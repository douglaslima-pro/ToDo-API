using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Claims;
using ToDo.API.Models.TaskList;
using ToDo.Application.Abstractions.Services;
using ToDo.Application.Features.Tasks.DTOs;

namespace ToDo.API.Controllers
{
    [Authorize]
    [Route("api/task-list")]
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

        [HttpPut("")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateTaskListModel model)
        {
            var taskList = new UpdateTaskListDTO
            {
                Id = model.Id,
                Title = model.Title ?? string.Empty,
            };

            var result = await _taskListService.UpdateAsync(taskList);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var result = await _taskListService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int start = 0, [FromQuery] int length = 10)
        {
            var user = User.ExtractClaims();

            if (user == null)
            {
                return Unauthorized();
            }

            var taskLists = await _taskListService.GetAllAsync(user.Id, start, length);

            return Ok(new
            {
                lists = taskLists
            });
        }
    }
}
