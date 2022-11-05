using NotificationService.DataAccess.DataBase.Entity;

namespace NotificationService.DataAccess.DataBase.Interfaces
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<MessageEntity>> GetAsync(int page, int count);
        public Task<MessageEntity> GetAsync(Guid id);
        public Task<Guid> AddAsync(MessageEntity entity);
    }
}
