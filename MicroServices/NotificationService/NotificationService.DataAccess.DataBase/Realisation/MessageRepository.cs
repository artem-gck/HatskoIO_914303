using Microsoft.EntityFrameworkCore;
using NotificationService.DataAccess.DataBase.Context;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Exceptions;
using NotificationService.DataAccess.DataBase.Interfaces;

namespace NotificationService.DataAccess.DataBase.Realisation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageContext _messageContext;

        public MessageRepository(MessageContext messageContext)
        {
            _messageContext = messageContext ?? throw new ArgumentNullException(nameof(messageContext));
        }

        public async Task<Guid> AddAsync(MessageEntity entity)
        {
            entity.CreatedDate = DateTime.Now;

            var messageDb = _messageContext.Messages.Add(entity);

            await _messageContext.SaveChangesAsync();

            return messageDb.Entity.Id;
        }

        public async Task<IEnumerable<MessageEntity>> GetAsync(int page, int count)
            => await _messageContext.Messages.OrderByDescending(mes => mes.CreatedDate).Skip((page - 1) * count).Take(count).ToListAsync();

        public async Task<MessageEntity> GetAsync(Guid id)
        {
            var entity = await _messageContext.Messages.FindAsync(id);

            if (entity is null)
                throw new NotFoundMessageException(id);

            return entity;
        }
    }
}
