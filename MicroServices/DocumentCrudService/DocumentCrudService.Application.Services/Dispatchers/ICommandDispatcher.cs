using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Application.Services.Dispatchers
{
    public interface ICommandDispatcher
    {
        void Send<T>(T command) where T : ICommand;
    }
}
