using AutoMapper;
using StructureService.Domain.Entities;
using StructureServiceApi.ViewModels.Responce;

namespace StructureServiceApi.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<DepartmentEntity, DepartmentResponce>();
            CreateMap<DepartmentResponce, DepartmentEntity>();

            CreateMap<DepartmentUnitEntity, DepartmentUnitResponce>().ForMember(dto => dto.Department, memb => memb.MapFrom(ent => ent.Department.Name))
                                                                .ForMember(dto => dto.CheifUserId, memb => memb.MapFrom(ent => ent.Department.CheifUserId))
                                                                .ForMember(dto => dto.Position, memb => memb.MapFrom(ent => ent.Position.Name));
            CreateMap<DepartmentUnitResponce, DepartmentUnitEntity>().ForMember(ent => ent.Department, memb => memb.MapFrom(dto => new DepartmentEntity { Name = dto.Department, CheifUserId = dto.CheifUserId }))
                                                                .ForMember(ent => ent.Position, memb => memb.MapFrom(dto => new PositionEntity() { Name = dto.Position }));

            CreateMap<PositionEntity, PositionResponce>();
            CreateMap<PositionResponce, PositionEntity>();
        }
    }
}
