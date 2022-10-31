using AutoMapper;
using StructureService.Domain.Entities;
using StructureServiceApi.ViewModels.AddRequest;
using StructureServiceApi.ViewModels.Responce;
using StructureServiceApi.ViewModels.UpdateRequest;

namespace StructureServiceApi.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<DepartmentEntity, DepartmentResponce>();
            CreateMap<DepartmentResponce, DepartmentEntity>();
            CreateMap<DepartmentEntity, UpdateDepartmentRequest>();
            CreateMap<UpdateDepartmentRequest, DepartmentEntity>();
            CreateMap<DepartmentEntity, AddDepartmentRequest>();
            CreateMap<AddDepartmentRequest, DepartmentEntity>();

            CreateMap<UserEntity, UserResponce>().ForMember(dto => dto.Department, memb => memb.MapFrom(ent => ent.Department.Name))
                                                 .ForMember(dto => dto.CheifUserId, memb => memb.MapFrom(ent => ent.Department.CheifUserId))
                                                 .ForMember(dto => dto.Position, memb => memb.MapFrom(ent => ent.Position.Name));
            CreateMap<UserResponce, UserEntity>().ForMember(ent => ent.Department, memb => memb.MapFrom(dto => new DepartmentEntity { Name = dto.Department, CheifUserId = dto.CheifUserId }))
                                                 .ForMember(ent => ent.Position, memb => memb.MapFrom(dto => new PositionEntity() { Name = dto.Position }));
            CreateMap<UserEntity, UpdateUserRequest>().ForMember(dto => dto.CheifUserId, memb => memb.MapFrom(ent => ent.Department.CheifUserId))
                                                      .ForMember(dto => dto.PositionId, memb => memb.MapFrom(ent => ent.Position.Id));
            CreateMap<UpdateUserRequest, UserEntity>().ForMember(ent => ent.Department, memb => memb.MapFrom(dto => new DepartmentEntity { CheifUserId = dto.CheifUserId }))
                                                      .ForMember(ent => ent.Position, memb => memb.MapFrom(dto => new PositionEntity() { Id = dto.PositionId }));
            CreateMap<UserEntity, AddUserRequest>().ForMember(dto => dto.CheifUserId, memb => memb.MapFrom(ent => ent.Department.CheifUserId))
                                                   .ForMember(dto => dto.Position, memb => memb.MapFrom(ent => ent.Position.Name));
            CreateMap<AddUserRequest, UserEntity>().ForMember(ent => ent.Department, memb => memb.MapFrom(dto => new DepartmentEntity { CheifUserId = dto.CheifUserId }))
                                                   .ForMember(ent => ent.Position, memb => memb.MapFrom(dto => new PositionEntity() { Name = dto.Position }));

            CreateMap<PositionEntity, PositionResponce>();
            CreateMap<PositionResponce, PositionEntity>();
            CreateMap<PositionEntity, UpdatePositionRequest>();
            CreateMap<UpdatePositionRequest, PositionEntity>();
            CreateMap<PositionEntity, AddPositionRequest>();
            CreateMap<AddPositionRequest, PositionEntity>();
        }
    }
}
