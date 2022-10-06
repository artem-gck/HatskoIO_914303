using AutoMapper;
using UsersService.Services.Dto;
using UsersServiceApi.VewModels;

namespace UsersServiceApi.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<UserInfoResponce, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfoResponce>();

            CreateMap<UpdateUserInfoRequest, UserInfoDto>();
            CreateMap<UserInfoDto, UpdateUserInfoRequest>();

            CreateMap<AddUserInfoRequest, UserInfoDto>();
            CreateMap<UserInfoDto, AddUserInfoRequest>();
        }
    }
}
