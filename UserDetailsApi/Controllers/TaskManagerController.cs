using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Interfaces;

namespace UserDetailsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController(ITaskManagerService taskInterface) : ControllerBase
    {
        [HttpPost("add-task")]
        public async Task<IActionResult> CreateTask(TaskDto taskModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
            var result = await taskInterface.CreateTask(userId, taskModel);
            if (result is not null)
            {
                return CreatedAtAction(nameof(CreateTask), new { id = result.Id }, result);
            }

            return BadRequest(result);
        }

        [HttpGet("get-tasks")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await taskInterface.GetTasks();
            return Ok(tasks);
        }

        [HttpGet("get-task/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await taskInterface.GetTaskById(id);

            if (task is null)
            {
                return NotFound(new ProblemDetails
                {
                    Type = "error",
                    Title = "Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No task with ID {id} exists."
                });
            }

            return Ok(task);
        }

        [HttpGet("get-task-by-user")]
        public async Task<IActionResult> GetTasksByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
            var task = await taskInterface.GetTasksByUserId(userId);

            if (task is null)
            {
                return NotFound(new ProblemDetails
                {
                    Type = "error",
                    Title = "Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No tasks found for user ID {userId}."
                });
            }

            return Ok(task);
        }

        [HttpPut("update-task/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto taskModel)
        {
            var result = await taskInterface.UpdateTask(id, taskModel);

            if (result is not null)
                return Ok(result);

            return NotFound(new ProblemDetails
            {
                Type = "error",
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"No task with ID {id} exists."
            });
        }

        [HttpDelete("delete-task/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await taskInterface.DeleteTask(id);
            if (!result)
            {
                return NotFound(new ProblemDetails
                {
                    Type = "error",
                    Title = "Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No task with ID {id} exists."
                });
            }

            return NoContent();
        }
    }
}
