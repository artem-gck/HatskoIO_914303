using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Commands
{
    public interface ICommandDispatcher
    {
        public Task Send<T>(T command) where T : ICommand;
    }
}
