using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Exceptions;
using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _service;

        public CommandDispatcher(IServiceProvider service)
        {
            _service = service;
        }

        public async Task Send<T>(T command) where T : ICommand
        {
            var handler = _service.GetService(typeof(ICommandHandler<T>));

            if (handler != null)
                await ((ICommandHandler<T>)handler).Handle(command);
            else
                throw new NotFoundServiceException($"Command doesn't have any handler {command.GetType().Name}");
        }
    }
}
