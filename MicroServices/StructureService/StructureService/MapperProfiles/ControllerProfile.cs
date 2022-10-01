using AutoMapper;
using StructureService.Application.Services.Dto;
using StructureService.ViewModels;

namespace StructureService.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<DepartmentDto, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, DepartmentDto>();
            
            CreateMap<DepartmentUnitDto, DepartmentUnitViewModel>();
            CreateMap<DepartmentUnitViewModel, DepartmentUnitDto>();
            
            CreateMap<PositionDto, PositionViewModel>();
            CreateMap<PositionViewModel, PositionDto>();
        }
    }
}
