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

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _messageContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (entity is null)
                throw new NotFoundMessageException(id);

            _messageContext.Tasks.Remove(entity);

            await _messageContext.SaveChangesAsync();
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

        public async Task UpdateAsync(Guid id, TaskEntity entity)
        {
            var entityDb = await _messageContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (entityDb is null)
                throw new NotFoundMessageException(id);

            entityDb.Header = entity.Header;
            entityDb.DeadLine = entity.DeadLine;
            entityDb.OwnerUserId = entity.OwnerUserId;

            await _messageContext.SaveChangesAsync();
        }
    }
}
