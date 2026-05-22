using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserDetailsApi.Data;
using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

namespace UserDetailsApi.Services
{
    public class TaskManagereService(UserDetailsDbContext context, ILogger<TaskManagereService> logger, IMapper mapper, ICacheService cache) : ITaskManagerService
    {
        private static string TaskKey(string key) => $"taskId={key}";
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
            var cachedTasks = await cache.GetCache<List<TaskResponseDto>>(TaskKey("allTasks"));
            if(cachedTasks is not null)
            {
                return (List<TaskResponseDto>)cachedTasks;
            }

            var tasks = await context.Tasks.ToListAsync();
            if (tasks is not null)
            {
                logger.LogInformation("Tasks retrieved");
                List<TaskResponseDto> taskListDto = [];

                foreach ( var item in tasks)
                {
                    taskListDto.Add(mapper.Map<TaskResponseDto>(item));
                }

                await cache.SetCache(TaskKey("allTasks"), taskListDto, TimeSpan.FromMinutes(5));
                return taskListDto;
            }                

            logger.LogInformation("No tasks retrieved");
            return null;
        }

        public async Task<List<TaskResponseDto>?> GetTasksByUserId(string userId)
        {
            logger.LogInformation("Retrieving all tasks for user with ID: {id}", userId);

            var cachedTasks = await cache.GetCache<List<TaskResponseDto>>(TaskKey(userId));
            if(cachedTasks is not null)
            {
                return cachedTasks;
            }

            var tasks = await context.Tasks.Where(p => p.UserId == userId).ToListAsync();
            if (tasks is not null)
            {
                logger.LogInformation("Tasks retrieved for user with ID: {id}", userId);
                List<TaskResponseDto> taskListDto = [];

                foreach (var item in tasks)
                {
                    taskListDto.Add(mapper.Map<TaskResponseDto>(item));
                }

                await cache.SetCache(TaskKey(userId), taskListDto, TimeSpan.FromMinutes(5));
                return taskListDto;
            }

            logger.LogInformation("No tasks retrieved");
            return null;
        }

        public async Task<TaskResponseDto?> GetTaskById(int id)
        {
            logger.LogInformation("Retrieving task with ID: {id}", id);

            var cachedTask = await cache.GetCache<TaskResponseDto>(TaskKey(id.ToString()));
            if (cachedTask is not null)
            {
                return cachedTask;
            }

            var taskModel = await context.Tasks.FindAsync(id);

            if (taskModel is null)
            {
                logger.LogInformation("Task with ID: {id} not found", id);
                return null;
            }

            logger.LogInformation("Task with ID: {id} retrieved", id);
            var task = mapper.Map<TaskResponseDto>(taskModel);
            await cache.SetCache(TaskKey(id.ToString()), task, TimeSpan.FromMinutes(5));

            return task;
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

            await cache.RemoveAsync(TaskKey(id.ToString()));

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
            await cache.RemoveAsync(TaskKey(id.ToString()));
            return true;
        }
    }
}
