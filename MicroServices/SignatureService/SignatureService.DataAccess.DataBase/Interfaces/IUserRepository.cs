using SignatureService.DataAccess.DataBase.Entities;

namespace SignatureService.DataAccess.DataBase.Interfaces
{
    public interface IUserRepository
    {
        public Task<Guid> AddUserAsync(UserEntity user);
    }
}
