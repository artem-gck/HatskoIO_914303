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
        public Task UpdatePositionAndDepartmentOfUserAsync(Guid id, UserEntity user);
        public Task<IEnumerable<UserEntity>> GetUsersByDepartmentId(Guid departmentId);
        public Task UpdateEmailOfUserAsync(Guid id, UserEntity user);
    }
}
