using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Models;

namespace UserDetailsApi.Interfaces
{
    public interface ITaskManagerService
    {
        Task<List<TaskResponseDto>?> GetTasks();
        Task<TaskResponseDto?> GetTaskById(int id);
        Task<List<TaskResponseDto>?> GetTasksByUserId(string userId);
        Task<TaskResponseDto?> UpdateTask(int id, TaskDto taskModel);
        Task<TaskResponseDto?> CreateTask(string userId, TaskDto taskModel);
        Task<bool> DeleteTask(int id);
    }
}
