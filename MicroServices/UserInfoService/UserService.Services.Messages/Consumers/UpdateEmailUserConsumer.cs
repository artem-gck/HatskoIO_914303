using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.DataAccess.Entities;
using UsersService.DataAccess;

namespace UsersService.Services.Messages.Consumers
{
    public class UpdateEmailUserConsumer : IConsumer<UpdateEmailUserMessage>
    {
        private readonly IUserRepositoty _userRepositoty;
        private readonly ILogger<UpdateEmailUserConsumer> _logger;

        public UpdateEmailUserConsumer(IUserRepositoty userRepositoty, ILogger<UpdateEmailUserConsumer> logger)
        {
            _userRepositoty = userRepositoty ?? throw new ArgumentNullException(nameof(userRepositoty));
            _logger = logger ?? throw new ArgumentNullException(nameof(userRepositoty));
        }

        public async Task Consume(ConsumeContext<UpdateEmailUserMessage> context)
        {
            var updateUser = context.Message;

            _logger.LogDebug("Update email user with id = {Id}", updateUser.Id);

            await _userRepositoty.UpdateEmailOfUserAsync(updateUser.Id, new UserEntity
            {
                Id = updateUser.Id,
                Email = updateUser.Email,
            });
        }
    }
}
