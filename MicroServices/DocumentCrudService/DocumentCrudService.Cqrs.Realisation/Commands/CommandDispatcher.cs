using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Exceptions;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Cqrs.Units;
using Microsoft.Extensions.Logging;

namespace DocumentCrudService.Cqrs.Realisation.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _service;
        private readonly ILogger<CommandDispatcher> _logger;

        public CommandDispatcher(IServiceProvider service, ILogger<CommandDispatcher> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult> Send<T>(T command) where T : ICommand
        {
            var handler = _service.GetService(typeof(ICommandHandler<T>));

            if (handler is ICommandHandler<T> commandHandler)
                return await commandHandler.Handle(command);
            else
            {
                var exception = new NotFoundServiceException($"Command doesn't have any handler {command.GetType().Name}");

                _logger.LogWarning(exception, "Not found service with name {Name}", command.GetType().Name);
                throw exception;
            }
        }
    }
}
