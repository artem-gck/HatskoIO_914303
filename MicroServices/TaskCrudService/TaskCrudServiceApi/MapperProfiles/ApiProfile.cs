using AutoMapper;
using TaskCrudService.Domain.Entities;
using TaskCrudServiceApi.ViewModels.CreateRequest;
using TaskCrudServiceApi.ViewModels.Responce;
using TaskCrudServiceApi.ViewModels.UpdateRequest;

namespace TaskCrudServiceApi.MapperProfiles
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<ArgumentEntity, ArgumentResponce>().ForMember(dto => dto.ArgumentType, memb => memb.MapFrom(ent => ent.ArgumentType.Name));
            CreateMap<ArgumentResponce, ArgumentEntity>().ForMember(ent => ent.ArgumentType, memb => memb.MapFrom(dto => new ArgumentTypeEntity() { Name = dto.ArgumentType }));
            CreateMap<ArgumentEntity, CreateArgumentRequest>().ForMember(dto => dto.ArgumentType, memb => memb.MapFrom(ent => ent.ArgumentType.Name));
            CreateMap<CreateArgumentRequest, ArgumentEntity>().ForMember(ent => ent.ArgumentType, memb => memb.MapFrom(dto => new ArgumentTypeEntity() { Name = dto.ArgumentType }));
            CreateMap<ArgumentEntity, UpdateArgumentRequest>().ForMember(dto => dto.ArgumentType, memb => memb.MapFrom(ent => ent.ArgumentType.Name));
            CreateMap<UpdateArgumentRequest, ArgumentEntity>().ForMember(ent => ent.ArgumentType, memb => memb.MapFrom(dto => new ArgumentTypeEntity() { Name = dto.ArgumentType }));

            CreateMap<TaskEntity, TaskResponce>().ForMember(dto => dto.Type, memb => memb.MapFrom(ent => ent.Type.Name));
            CreateMap<TaskResponce, TaskEntity>().ForMember(ent => ent.Type, memb => memb.MapFrom(dto => new TypeEntity() { Name = dto.Type }));
            CreateMap<TaskEntity, CreateTaskRequest>().ForMember(dto => dto.Type, memb => memb.MapFrom(ent => ent.Type.Name));
            CreateMap<CreateTaskRequest, TaskEntity>().ForMember(ent => ent.Type, memb => memb.MapFrom(dto => new TypeEntity() { Name = dto.Type }));
            CreateMap<TaskEntity, UpdateTaskRequest>().ForMember(dto => dto.Type, memb => memb.MapFrom(ent => ent.Type.Name));
            CreateMap<UpdateTaskRequest, TaskEntity>().ForMember(ent => ent.Type, memb => memb.MapFrom(dto => new TypeEntity() { Name = dto.Type }));
        }
    }
}
