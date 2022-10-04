using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Application.Services.Commands
{
    public interface ICommandHandler<T> : ICommandHandler where T : ICommand
    {
        Task Handle(T command);
    }
}
