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

            CreateMap<DepartmentUnitEntity, DepartmentUnitDto>().ForMember(dto => dto.Department, memb => memb.MapFrom(ent => ent.Department.Name))
                                                                .ForMember(dto => dto.CheifUserId, memb => memb.MapFrom(ent => ent.Department.CheifUserId))
                                                                .ForMember(dto => dto.Position, memb => memb.MapFrom(ent => ent.Position.Name));
            CreateMap<DepartmentUnitDto, DepartmentUnitEntity>().ForMember(ent => ent.Department, memb => memb.MapFrom(dto => new DepartmentEntity { Name = dto.Department, CheifUserId = dto.CheifUserId }))
                                                                .ForMember(ent => ent.Position, memb => memb.MapFrom(dto => new PositionEntity() { Name = dto.Position }));

            CreateMap<PositionEntity, PositionDto>();
            CreateMap<PositionDto, PositionEntity>();
        }
    }
}
