using AutoMapper;
using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Models;
using UserDetailsApi.Models.RequestModels.TaskRequestModels;

namespace UserDetailsApi.Helpers
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskModel, TaskResponseDto>();
            CreateMap<TaskDto, TaskModel>();
            CreateMap<UpdateTaskRequestModel, TaskDto>();
            CreateMap<CreateTaskRequestModel, TaskDto>();
        }
    }
}
