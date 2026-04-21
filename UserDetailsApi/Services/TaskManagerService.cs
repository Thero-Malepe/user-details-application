using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserDetailsApi.Data;
using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

namespace UserDetailsApi.Services
{
    public class TaskManagereService(UserDetailsDbContext context, ILogger<TaskManagereService> logger, IMapper mapper) : ITaskManagerService
    {
        public async Task<TaskResponseDto?> CreateTask(string userId, TaskDto taskModel)
        {
            logger.LogInformation("Creating task");

            TaskModel newTask = new()
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
                Status = taskModel.Status,
                Priority = taskModel.Priority,
                DueDate = taskModel.DueDate?.ToUniversalTime(),
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
            };

            context.Tasks.Add(newTask);
            var rowsAffected = await context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                logger.LogInformation("Task created with ID: {newTask.Id}", newTask.Id);
                return mapper.Map<TaskResponseDto>(newTask);
            }

            logger.LogInformation("Task not created");
            return null;
        }

        public async Task<List<TaskResponseDto>?> GetTasks()
        {
            logger.LogInformation("Retrieving all tasks");
            var tasks = await context.Tasks.ToListAsync();

            if (tasks is not null)
            {
                logger.LogInformation("Tasks retrieved");
                List<TaskResponseDto> taskListDto = [];

                foreach ( var item in tasks)
                {
                    taskListDto.Add(mapper.Map<TaskResponseDto>(item));
                }
                return taskListDto;
            }                

            logger.LogInformation("No tasks retrieved");
            return null;
        }

        public async Task<List<TaskResponseDto>?> GetTasksByUserId(string userId)
        {
            logger.LogInformation("Retrieving all tasks for user with ID: {id}", userId);
            var tasks = await context.Tasks.Where(p => p.UserId == userId).ToListAsync();

            if (tasks is not null)
            {
                logger.LogInformation("Tasks retrieved for user with ID: {id}", userId);
                List<TaskResponseDto> taskListDto = [];

                foreach (var item in tasks)
                {
                    taskListDto.Add(mapper.Map<TaskResponseDto>(item));
                }
                return taskListDto;
            }

            logger.LogInformation("No tasks retrieved");
            return null;
        }

        public async Task<TaskResponseDto?> GetTaskById(int id)
        {
            logger.LogInformation("Retrieving task with ID: {id}", id);
            var taskModel = await context.Tasks.FindAsync(id);

            if (taskModel is null)
            {
                logger.LogInformation("Task with ID: {id} not found", id);
                return null;
            }

            logger.LogInformation("Task with ID: {id} retrieved", id);
            return mapper.Map<TaskResponseDto>(taskModel);
        }

        public async Task<TaskResponseDto?> UpdateTask(int id, TaskDto taskModel)
        {
            if (id <= 0)
            {
                logger.LogError("Invalid Id");
                return null;
            }
                
            logger.LogInformation("Updating task with ID: {taskModel.Id}", id);

            var existingTask = await context.Tasks.FindAsync(id);
            if (existingTask is null)
            {
                logger.LogInformation("Task with ID: {id} not found", id);
                return null;
            }

            taskModel.DueDate = taskModel.DueDate?.ToUniversalTime();
            mapper.Map(taskModel, existingTask);
            await context.SaveChangesAsync();
            logger.LogInformation("Task with ID: {id} updated", id);

            return mapper.Map<TaskResponseDto>(existingTask);            
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
