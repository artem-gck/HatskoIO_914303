using Microsoft.EntityFrameworkCore;
using UsersService.DataAccess.Entities;
using UsersService.DataAccess.Entities.Context;

namespace UsersService.DataAccess
{
    public class UserAccess : IUserAccess
    {
        private readonly UsersInfoContext _usersContext;

        public UserAccess(UsersInfoContext usersContext)
            => _usersContext = usersContext;

        public async Task<int> AddUserInfoAsync(UserInfoEntity userInfo)
        {
            var userInfoEntity = _usersContext.UsersInfo.Add(userInfo);

            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Entity.Id;
        }

        public async Task<int> DeleteUserInfoAsync(int id)
        {
            var userInfoEntity = await _usersContext.UsersInfo.FirstAsync(us => us.Id == id);

            var deletedUserInfoEntity = _usersContext.UsersInfo.Remove(userInfoEntity);

            await _usersContext.SaveChangesAsync();

            return deletedUserInfoEntity.Entity.Id;
        }

        public async Task<UserInfoEntity> GetUserInfoAsync(int id)
            => await _usersContext.UsersInfo.FirstAsync(us => us.Id == id);

        public async Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync()
            => await _usersContext.UsersInfo.ToArrayAsync();

        public async Task<int> UpdateUserInfoAsync(UserInfoEntity userInfo)
        {
            var userInfoEntity = await _usersContext.UsersInfo.FirstAsync(us => us.Id == userInfo.Id);

            userInfoEntity.Name = userInfo.Name;
            userInfoEntity.Surname = userInfo.Surname;
            userInfoEntity.Patronymic = userInfo.Patronymic;
            userInfoEntity.Email = userInfo.Email;

            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Id;
        }
    }
}
