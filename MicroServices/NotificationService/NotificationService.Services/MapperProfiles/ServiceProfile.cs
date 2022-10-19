using AutoMapper;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.Services.Dto;

namespace NotificationService.Services.MapperProfiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<MessageEntity, MessageDto>();
        }
    }
}
