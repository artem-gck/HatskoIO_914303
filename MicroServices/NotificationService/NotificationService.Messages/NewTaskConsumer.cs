using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Messages
{
    public class NewTaskConsumer : IConsumer<NewTaskMessage>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<NewTaskConsumer> _logger;

        public NewTaskConsumer(ITaskRepository taskRepository, ILogger<NewTaskConsumer> logger)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<NewTaskMessage> context)
        {
            var newTask = context.Message;

            _logger.LogDebug("Create new task with id = {Id}", newTask.Id);

            var taskEntity = new TaskEntity
            {
                Id = newTask.Id,
                OwnerUserId = newTask.OwnerUserId,
                Header = newTask.Header,
                DeadLine = newTask.DeadLine
            };

            await _taskRepository.AddAsync(taskEntity);
        }
    }
}
