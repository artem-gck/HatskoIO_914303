using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Interfaces;

namespace NotificationService.Messages
{
    public class UpdateTaskConsumer : IConsumer<UpdateTaskMessage>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<UpdateTaskConsumer> _logger;

        public UpdateTaskConsumer(ITaskRepository taskRepository, ILogger<UpdateTaskConsumer> logger)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<UpdateTaskMessage> context)
        {
            var newTask = context.Message;

            _logger.LogDebug("Update task with id = {Id}", newTask.Id);

            var taskEntity = new TaskEntity
            {
                Id = newTask.Id,
                OwnerUserId = newTask.OwnerUserId,
                Header = newTask.Header,
                DeadLine = newTask.DeadLine
            };

            await _taskRepository.UpdateAsync(taskEntity.Id, taskEntity);
        }
    }
}
