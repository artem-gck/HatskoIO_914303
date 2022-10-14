using AutoMapper;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.Services.Dto;

namespace CompanyManagementService.Services.MapperProfiles
{
    public class ServicesProfile : Profile
    {
        public ServicesProfile()
        {
            CreateMap<(UserResponce, DataAccess.UserEntity.UserResponce), UserDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Item2.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Item2.Name))
                .ForMember(d => d.Surname, opt => opt.MapFrom(s => s.Item2.Surname))
                .ForMember(d => d.Patronymic, opt => opt.MapFrom(s => s.Item2.Patronymic))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Item2.Email))
                .ForMember(d => d.Position, opt => opt.MapFrom(s => s.Item1.Position));

            CreateMap<(PositionResponce, DataAccess.UserEntity.UserResponce), UserDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Item2.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Item2.Name))
                .ForMember(d => d.Surname, opt => opt.MapFrom(s => s.Item2.Surname))
                .ForMember(d => d.Patronymic, opt => opt.MapFrom(s => s.Item2.Patronymic))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Item2.Email))
                .ForMember(d => d.Position, opt => opt.MapFrom(s => s.Item1.Name));
        }
    }
}
