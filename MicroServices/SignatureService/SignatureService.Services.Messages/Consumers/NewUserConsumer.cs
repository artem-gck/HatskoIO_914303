using MassTransit;
<<<<<<< Updated upstream
using SignatureService.Services.Interfaces;
using SignatureService.Services.Messages.Messages;
=======
using Messages;
using Microsoft.Extensions.Logging;
using SignatureService.Services.Interfaces;
>>>>>>> Stashed changes

namespace SignatureService.Services.Messages.Consumers
{
    public class NewUserConsumer : IConsumer<NewUserMessage>
    {
        private readonly IUserService _userService;
<<<<<<< Updated upstream

        public NewUserConsumer(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
=======
        private readonly ILogger<NewUserConsumer> _logger;

        public NewUserConsumer(IUserService userService, ILogger<NewUserConsumer> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
>>>>>>> Stashed changes
        }

        public async Task Consume(ConsumeContext<NewUserMessage> context)
        {
            var newUser = context.Message;

<<<<<<< Updated upstream
=======
            _logger.LogInformation("Create user with id = {Id}", newUser.Id);

>>>>>>> Stashed changes
            await _userService.AddUserAsync(newUser.Id);
        }
    }
}
