using AutoMapper;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Ports.Output.Dto;

namespace TaskCrudService.Adapters.Output.MapperProfiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<ArgumentEntity, ArgumentDto>().ForMember(dto => dto.ArgumentType, memb => memb.MapFrom(ent => ent.ArgumentType.Name));
            CreateMap<ArgumentDto, ArgumentEntity>().ForMember(ent => ent.ArgumentType, memb => memb.MapFrom(dto => new ArgumentTypeEntity() { Name = dto.ArgumentType }));

            CreateMap<TaskEntity, TaskDto>().ForMember(dto => dto.Type, memb => memb.MapFrom(ent => ent.Type.Name));
            CreateMap<TaskDto, TaskEntity>().ForMember(ent => ent.Type, memb => memb.MapFrom(dto => new TypeEntity() { Name = dto.Type }));
        }
    }
}
