using UsersService.DataAccess.Entities;

namespace UsersService.DataAccess
{
    public interface IUserInfoRepositoty
    {
        public Task<IEnumerable<UserEntity>> GetUsersInfoAsync();
        public Task<UserEntity> GetUserInfoAsync(Guid id);
        public Task DeleteUserInfoAsync(Guid id);
        public Task<Guid> AddUserInfoAsync(UserEntity userInfo);
        public Task UpdateUserInfoAsync(Guid id, UserEntity userInfo);
    }
}
