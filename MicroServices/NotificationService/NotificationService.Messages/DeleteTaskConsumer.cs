using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Interfaces;

namespace NotificationService.Messages
{
    public class DeleteTaskConsumer : IConsumer<DeleteTaskMessage>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<DeleteTaskConsumer> _logger;

        public DeleteTaskConsumer(ITaskRepository taskRepository, ILogger<DeleteTaskConsumer> logger)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<DeleteTaskMessage> context)
        {
            var newTask = context.Message;

            _logger.LogDebug("Delete task with id = {Id}", newTask.Id);

            await _taskRepository.DeleteAsync(newTask.Id);
        }
    }
}
