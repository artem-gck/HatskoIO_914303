using UsersService.DataAccess.Entities;

namespace UsersService.DataAccess
{
    public interface IUserAccess
    {
        public Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync();
        public Task<UserInfoEntity> GetUserInfoAsync(int id);
        public Task<int> DeleteUserInfoAsync(int id);
        public Task<int> AddUserInfoAsync(UserInfoEntity userInfo);
        public Task<int> UpdateUserInfoAsync(int id, UserInfoEntity userInfo);
    }
}
