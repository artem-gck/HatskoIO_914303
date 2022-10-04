using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Application.Services.Commands
{
    public interface ICommandDispatcher
    {
        public Task Send<T>(T command) where T : ICommand;
    }
}
