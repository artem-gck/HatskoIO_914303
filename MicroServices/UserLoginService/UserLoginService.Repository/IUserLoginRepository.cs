using UserLoginService.Domain.Entities;

namespace UserLoginService.Repository
{
    public interface IUserLoginRepository
    {
        public Task<IEnumerable<UserLoginEntity>> GetAllUsersLoginAsync();
        public Task<UserLoginEntity> GetUserLoginAsync(int id);
        public Task<int> InsertUserLoginAsync(UserLoginEntity userLoginEntity);
        public Task<int> UpdateUserLoginAsync(int id, UserLoginEntity userLoginEntity);
        public Task<int> RemoveUserLoginAsync(int id);
    }
}
