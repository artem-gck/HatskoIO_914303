using AutoMapper;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.Services.Dto;
using NotificationServiceApi.ViewModels;

namespace NotificationServiceApi.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<MessageDto, MessageResponce>();
        }
    }
}
