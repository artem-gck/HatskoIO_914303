using AutoMapper;
using StructureService.Application.Services.Dto;
using StructureService.Domain.Entities;

namespace StructureService.Application.Realisation.MapperProfiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<DepartmentEntity, DepartmentDto>();
            CreateMap<DepartmentDto, DepartmentEntity>();

            CreateMap<DepartmentUnitEntity, DepartmentUnitDto>();
            CreateMap<DepartmentUnitDto, DepartmentUnitEntity>();

            CreateMap<PositionEntity, PositionDto>();
            CreateMap<PositionDto, PositionEntity>();
        }
    }
}
