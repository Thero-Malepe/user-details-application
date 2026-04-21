using AutoMapper;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.Models.RequestModels.UserRequestModels;

namespace UserDetailsApi.Helpers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserRequestModel, UserDto>();
            CreateMap<ResetPasswordRequestModel, ResetDto>();
            CreateMap<LoginUserRequestModel, LoginDto>();
            CreateMap<RefreshTokenRequestModel, TokenResponseDto>();
        }
    }
}
