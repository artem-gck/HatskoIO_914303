using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Interfaces;

namespace NotificationService.Messages
{
    public class NewUserConsumer : IConsumer<NewUserMessage>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<NewUserConsumer> _logger;

        public NewUserConsumer(IUserRepository userRepository, ILogger<NewUserConsumer> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<NewUserMessage> context)
        {
            var newUser = context.Message;

            _logger.LogDebug("Create new user with id = {Id}", newUser.Id);

            var userEntity = new UserEntity
            {
                Id = newUser.Id,
                Email = newUser.Email
            };

            await _userRepository.AddAsync(userEntity);
        }
    }
}