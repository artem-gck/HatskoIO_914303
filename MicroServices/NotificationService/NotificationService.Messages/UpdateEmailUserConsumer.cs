using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Interfaces;

namespace NotificationService.Messages
{
    public class UpdateEmailUserConsumer : IConsumer<UpdateEmailUserMessage>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateEmailUserConsumer> _logger;

        public UpdateEmailUserConsumer(IUserRepository userRepository, ILogger<UpdateEmailUserConsumer> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<UpdateEmailUserMessage> context)
        {
            var userMessage = context.Message;

            _logger.LogDebug("Update user with id = {Id}", userMessage.Id);

            var user = new UserEntity
            {
                Id = userMessage.Id,
                Email = userMessage.Email,
            };

            await _userRepository.UpdateEmailAsync(userMessage.Id, user);
        }
    }
}
