using AutoMapper;
using TaskCrudService.Application.Services.Dto;
using TaskCrudService.ViewModels;

namespace TaskCrudService.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<ArgumentDto, ArgumentViewModel>();
            CreateMap<ArgumentViewModel, ArgumentDto>();

            CreateMap<TaskDto, TaskViewModel>();
            CreateMap<TaskViewModel, TaskDto>();
        }
    }
}
