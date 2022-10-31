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
    public class UpdateUserConsumer : IConsumer<UpdateUserMessage>
    {
        private readonly IUserRepositoty _userRepositoty;
        private readonly ILogger<UpdateUserConsumer> _logger;

        public UpdateUserConsumer(IUserRepositoty userRepositoty, ILogger<UpdateUserConsumer> logger)
        {
            _userRepositoty = userRepositoty ?? throw new ArgumentNullException(nameof(userRepositoty));
            _logger = logger ?? throw new ArgumentNullException(nameof(userRepositoty));
        }

        public async Task Consume(ConsumeContext<UpdateUserMessage> context)
        {
            var updateUser = context.Message;

            _logger.LogDebug("Update user with id = {Id}", updateUser.Id);

            await _userRepositoty.UpdatePositionAndDepartmentOfUserAsync(updateUser.Id, new UserEntity
            {
                Id = updateUser.Id,
                PositionId = updateUser.PositionId,
                DepartmentId = updateUser.DepartmentId,
            });
        }
    }
}
