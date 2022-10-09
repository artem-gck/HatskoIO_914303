using AutoMapper;
using UsersService.Services.Dto;
using UsersServiceApi.VewModels;

namespace UsersServiceApi.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<UserResponce, UserInfoDto>();
            CreateMap<UserInfoDto, UserResponce>();

            CreateMap<UpdateUserRequest, UserInfoDto>();
            CreateMap<UserInfoDto, UpdateUserRequest>();

            CreateMap<AddUserRequest, UserInfoDto>();
            CreateMap<UserInfoDto, AddUserRequest>();
        }
    }
}
