using AutoMapper;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.MapperProfiles
{
    /// <summary>
    /// Profile for controller mappers.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ControllerProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerProfile"/> class.
        /// </summary>unnecessary
        public ControllerProfile()
        {
            CreateMap<UserInfoViewModel, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfoViewModel>();
        }
    }
}
