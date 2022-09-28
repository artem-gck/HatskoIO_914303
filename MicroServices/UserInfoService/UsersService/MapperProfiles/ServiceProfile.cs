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
            CreateMap<UserInfoEntity, UserInfoDto>().ForMember(dest => dest.Position, act => act.MapFrom(src => src.Position.Name));
            CreateMap<UserInfoDto, UserInfoEntity>().ForMember(dest => dest.Position, act => act.MapFrom(src => new PositionEntity() { Name = src.Position }));
        }
    }
}
