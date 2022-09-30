using AutoMapper;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<UserInfoViewModel, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfoViewModel>();
        }
    }
}
