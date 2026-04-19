using AutoMapper;
using UserDetailsApi.DTOs.TaskManagerDtos;
using UserDetailsApi.Models;

namespace UserDetailsApi.Helpers
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskModel, TaskResponseDto>();
            CreateMap<TaskDto, TaskModel>();
        }
    }
}
