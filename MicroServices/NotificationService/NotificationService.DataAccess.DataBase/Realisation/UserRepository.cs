using NotificationService.DataAccess.DataBase.Context;
using NotificationService.DataAccess.DataBase.Entity;
using NotificationService.DataAccess.DataBase.Exceptions;
using NotificationService.DataAccess.DataBase.Interfaces;

namespace NotificationService.DataAccess.DataBase.Realisation
{
    public class UserRepository : IUserRepository
    {
        private readonly MessageContext _messageContext;

        public UserRepository(MessageContext messageContext)
        {
            _messageContext = messageContext ?? throw new ArgumentNullException(nameof(messageContext));
        }

        public async Task<Guid> AddAsync(UserEntity entity)
        {
            var userDb = _messageContext.Users.Add(entity);

            await _messageContext.SaveChangesAsync();

            return userDb.Entity.Id;
        }

        public async Task<UserEntity> GetAsync(Guid id)
        {
            var entity = await _messageContext.Users.FindAsync(id);

            if (entity is null)
                throw new NotFoundMessageException(id);

            return entity;
        }

        public async Task UpdateEmailAsync(Guid id, UserEntity entity)
        {
            var entityDb = await _messageContext.Users.FindAsync(id);

            if (entityDb is null)
                throw new NotFoundMessageException(id);

            entityDb.Email = entity.Email;

            await _messageContext.SaveChangesAsync();
        }
    }
}
