using Microsoft.EntityFrameworkCore;
using NotificationService.DataAccess.DataBase.Context;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Exceptions;
using NotificationService.DataAccess.DataBase.Interfaces;

namespace NotificationService.DataAccess.DataBase.Realisation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly MessageContext _messageContext;

        public TaskRepository(MessageContext messageContext)
        {
            _messageContext = messageContext ?? throw new ArgumentNullException(nameof(messageContext));
        }

        public async Task<Guid> AddAsync(TaskEntity entity)
        {
            var taskDb = _messageContext.Tasks.Add(entity);

            await _messageContext.SaveChangesAsync();

            return taskDb.Entity.Id;
        }

        public async Task<IEnumerable<IGrouping<Guid, TaskEntity>>> GetAsync()
            => await _messageContext.Tasks.Where(task => (task.DeadLine - DateTime.Now) <= TimeSpan.FromDays(1))
                                          .GroupBy(task => task.OwnerUserId)
                                          .ToListAsync();

        public async Task<TaskEntity> GetAsync(Guid id)
        {
            var entity = await _messageContext.Tasks.FindAsync(id);

            if (entity is null)
                throw new NotFoundMessageException(id);

            return entity;
        }
    }
}
