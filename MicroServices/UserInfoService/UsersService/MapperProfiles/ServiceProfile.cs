using AutoMapper;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.MapperProfiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<UserInfoEntity, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfoEntity>();
        }
    }
}
