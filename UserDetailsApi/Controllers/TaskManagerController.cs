using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

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
            var result = await taskInterface.CreateTask(taskModel);
            if (result is not null)
            {
                return CreatedAtAction(nameof(CreateTask), new { id = result.Id }, result);
            }

            return BadRequest(result);
        }

        [HttpGet("get-tasks")]
        public async Task<IActionResult> GetTasks()
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

        [HttpPut("update-task/")]
        public async Task<IActionResult> UpdateTask(TaskModel taskModel)
        {
            var result = await taskInterface.UpdateTask(taskModel);

            if (result is not null)
                return Ok(result);

            return NotFound(new ProblemDetails
            {
                Type = "error",
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"No task with ID {taskModel.Id} exists."
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
