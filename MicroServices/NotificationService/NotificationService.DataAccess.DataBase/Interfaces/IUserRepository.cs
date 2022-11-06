using NotificationService.DataAccess.DataBase.Entity;

namespace NotificationService.DataAccess.DataBase.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserEntity> GetAsync(Guid id);
        public Task<Guid> AddAsync(UserEntity entity);
        public Task UpdateEmailAsync(Guid id, UserEntity entity);
    }
}
