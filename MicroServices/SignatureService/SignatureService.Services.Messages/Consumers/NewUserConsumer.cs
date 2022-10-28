using MassTransit;
using SignatureService.Services.Interfaces;
using SignatureService.Services.Messages.Messages;

namespace SignatureService.Services.Messages.Consumers
{
    public class NewUserConsumer : IConsumer<NewUserMessage>
    {
        private readonly IUserService _userService;

        public NewUserConsumer(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task Consume(ConsumeContext<NewUserMessage> context)
        {
            var newUser = context.Message;

            await _userService.AddUserAsync(newUser.Id);
        }
    }
}
