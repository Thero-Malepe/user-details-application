using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Models;

namespace UserDetailsApi.Interfaces
{
    public interface ITaskManagerService
    {
        Task<List<TaskModel>?> GetTasks();
        Task<TaskModel?> GetTaskById(int id);
        Task<TaskModel?> UpdateTask(TaskModel taskModel);
        Task<TaskModel?> CreateTask(TaskDto taskModel);
        Task<bool> DeleteTask(int id);
    }
}
