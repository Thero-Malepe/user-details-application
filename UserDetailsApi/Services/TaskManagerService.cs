using Microsoft.EntityFrameworkCore;
using UserDetailsApi.Data;
using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

namespace UserDetailsApi.Services
{
    public class TaskManagereService(UserDetailsDbContext context, ILogger<TaskManagereService> logger) : ITaskManagerService
    {
        public async Task<TaskModel?> CreateTask(TaskDto taskModel)
        {
            logger.LogInformation("Creating task");
            var newTask = new TaskModel
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
                Status = taskModel.Status,
                Priority = taskModel.Priority,
                DueDate = taskModel.DueDate?.ToUniversalTime(),
                CreatedAt = DateTime.UtcNow
            };

            context.Tasks.Add(newTask);
            var rowsAffected = await context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                logger.LogInformation("Task created with ID: {newTask.Id}", newTask.Id);
                return newTask;
            }

            logger.LogInformation("Task not created");
            return null;
        }

        public async Task<List<TaskModel>?> GetTasks()
        {
            logger.LogInformation("Retrieving all tasks");
            var tasks = await context.Tasks.ToListAsync();

            if (tasks is not null)
            {
                logger.LogInformation("Tasks retrieved");
                return tasks;
            }                

            logger.LogInformation("No tasks retrieved");
            return null;
        }

        public async Task<TaskModel?> GetTaskById(int id)
        {
            logger.LogInformation("Retrieving task with ID: {tt}", id);
            var taskModel = await context.Tasks.FindAsync(id);

            if (taskModel is null)
            {
                logger.LogInformation("Task with ID: {id} not found", id);
                return null;
            }

            logger.LogInformation("Task with ID: {id} retrieved", id);
            return taskModel;
        }

        public async Task<TaskModel?> UpdateTask(TaskModel taskModel)
        {
            logger.LogInformation("Updating task with ID: {taskModel.Id}", taskModel.Id);

            var existingTask = await context.Tasks.FindAsync(taskModel.Id);
            if (existingTask is null)
            {
                logger.LogInformation("Task with ID: {id} not found", taskModel.Id);
                return null;
            }

            existingTask.Title = taskModel.Title;
            existingTask.Description = taskModel.Description;
            existingTask.Status = taskModel.Status;
            existingTask.Priority = taskModel.Priority;
            existingTask.DueDate = taskModel.DueDate?.ToUniversalTime();

            await context.SaveChangesAsync();

            logger.LogInformation("Task with ID: {id} updated", taskModel.Id);
            return existingTask;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var affected = await context.Tasks.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (affected == 0)
            {
                logger.LogInformation("Task with ID: {id} not found", id);
                return false;
            }

            logger.LogInformation("Task with ID: {id} deleted", id);
            return true;
        }
    }
}
