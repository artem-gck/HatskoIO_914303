using AutoMapper;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.MapperProfiles
{
    /// <summary>
    /// Profile for service mappers.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ServiceProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProfile"/> class.
        /// </summary>
        public ServiceProfile()
        {
            CreateMap<UserInfoEntity, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfoEntity>();
        }
    }
}
