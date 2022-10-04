using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Application.Services.Handlers
{
    public interface ICommandHandler<T> : ICommandHandler where T : ICommand
    {
        Task Handle(T command);
    }
}
