using AutoMapper;
using UsersService.Services.Dto;
using UsersServiceApi.VewModels;

namespace UsersServiceApi.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<UserResponce, UserDto>();
            CreateMap<UserDto, UserResponce>();

            CreateMap<UpdateUserRequest, UserDto>();
            CreateMap<UserDto, UpdateUserRequest>();

            CreateMap<AddUserRequest, UserDto>();
            CreateMap<UserDto, AddUserRequest>();
        }
    }
}
