using AutoMapper;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services.MapperProfiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<UserEntity, UserInfoDto>();
            CreateMap<UserInfoDto, UserEntity>();
        }
    }
}
