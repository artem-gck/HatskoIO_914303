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
            CreateMap<PerformerEntity, PerformerResponse>();
            CreateMap<PerformerResponse, PerformerEntity>();
            CreateMap<PerformerEntity, CreatePerformerRequest>();
            CreateMap<CreatePerformerRequest, PerformerEntity>();
            CreateMap<PerformerEntity, UpdatePerformerRequest>();
            CreateMap<UpdatePerformerRequest, PerformerEntity>();

            CreateMap<DocumentEntity, DocumentResponse>();
            CreateMap<DocumentResponse, DocumentEntity>();
            CreateMap<DocumentEntity, CreateDocumentRequest>();
            CreateMap<CreateDocumentRequest, DocumentEntity>();
            CreateMap<DocumentEntity, UpdateDocumentRequest>();
            CreateMap<UpdateDocumentRequest, DocumentEntity>();

            CreateMap<TaskEntity, TaskResponce>().ForMember(dto => dto.Type, memb => memb.MapFrom(ent => ent.Type.Name));
            CreateMap<TaskResponce, TaskEntity>().ForMember(ent => ent.Type, memb => memb.MapFrom(dto => new TypeEntity() { Name = dto.Type }));
            CreateMap<TaskEntity, CreateTaskRequest>().ForMember(dto => dto.Type, memb => memb.MapFrom(ent => ent.Type.Name));
            CreateMap<CreateTaskRequest, TaskEntity>().ForMember(ent => ent.Type, memb => memb.MapFrom(dto => new TypeEntity() { Name = dto.Type }));
            CreateMap<TaskEntity, UpdateTaskRequest>().ForMember(dto => dto.Type, memb => memb.MapFrom(ent => ent.Type.Name));
            CreateMap<UpdateTaskRequest, TaskEntity>().ForMember(ent => ent.Type, memb => memb.MapFrom(dto => new TypeEntity() { Name = dto.Type }));
        }
    }
}
