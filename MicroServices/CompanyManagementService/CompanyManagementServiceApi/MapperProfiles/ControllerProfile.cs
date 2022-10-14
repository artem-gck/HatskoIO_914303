using AutoMapper;
using CompanyManagementService.Services.Dto;
using CompanyManagementServiceApi.ViewModels;

namespace CompanyManagementServiceApi.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<UserDto, UserResponce>();
            CreateMap<CheifStructureDto, CheifStructureResponce>();
        }
    }
}
