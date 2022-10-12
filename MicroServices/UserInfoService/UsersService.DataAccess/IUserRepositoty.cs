using UsersService.DataAccess.Entities;

namespace UsersService.DataAccess
{
    public interface IUserRepositoty
    {
        public Task<IEnumerable<UserEntity>> GetUsersAsync();
        public Task<UserEntity> GetUserAsync(Guid id);
        public Task DeleteUserAsync(Guid id);
        public Task<Guid> AddUserAsync(UserEntity user);
        public Task UpdateUserAsync(Guid id, UserEntity user);
    }
}
