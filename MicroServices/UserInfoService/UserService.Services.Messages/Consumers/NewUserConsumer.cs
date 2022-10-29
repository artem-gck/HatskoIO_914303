using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities;

namespace UsersService.Services.Messages.Consumers
{
    public class NewUserConsumer : IConsumer<NewUserMessage>
    {
        private readonly IUserRepositoty _userRepositoty;
        private readonly ILogger<NewUserConsumer> _logger;

        public NewUserConsumer(IUserRepositoty userRepositoty, ILogger<NewUserConsumer> logger)
        {
            _userRepositoty = userRepositoty ?? throw new ArgumentNullException(nameof(userRepositoty));
            _logger = logger ?? throw new ArgumentNullException(nameof(userRepositoty));
        }

        public async Task Consume(ConsumeContext<NewUserMessage> context)
        {
            var newUser = context.Message;

            _logger.LogDebug("Create new user with id = {Id}", newUser.Id);

            await _userRepositoty.AddUserAsync(new UserEntity
            {
                Id = newUser.Id,
            });
        }
    }
}
