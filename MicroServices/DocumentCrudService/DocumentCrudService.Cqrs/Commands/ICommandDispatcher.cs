using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Commands
{
    public interface ICommandDispatcher
    {
        public Task<IResult> Send<T>(T command) where T : ICommand;
    }
}
