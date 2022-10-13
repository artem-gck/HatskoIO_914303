using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Commands
{
    public interface ICommandHandler<T> : ICommandHandler where T : ICommand
    {
        Task<IResult> Handle(T command);
    }
}
