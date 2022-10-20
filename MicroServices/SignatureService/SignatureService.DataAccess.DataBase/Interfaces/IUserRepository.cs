using SignatureService.DataAccess.DataBase.Entities;

namespace SignatureService.DataAccess.DataBase.Interfaces
{
    public interface IUserRepository
    {
        public Task<Guid> AddAsync(UserEntity user);
        public Task<UserEntity> GetAsync(Guid id);
    }
}
