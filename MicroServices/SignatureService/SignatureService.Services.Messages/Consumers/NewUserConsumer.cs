using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using SignatureService.Services.Interfaces;

namespace SignatureService.Services.Messages.Consumers
{
    public class NewUserConsumer : IConsumer<NewUserMessage>
    {
        private readonly IUserService _userService;
        private readonly ILogger<NewUserConsumer> _logger;

        public NewUserConsumer(IUserService userService, ILogger<NewUserConsumer> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<NewUserMessage> context)
        {
            var newUser = context.Message;

            _logger.LogDebug("Create user with id = {Id}", newUser.Id);

            await _userService.AddUserAsync(newUser.Id);
        }
    }
}
