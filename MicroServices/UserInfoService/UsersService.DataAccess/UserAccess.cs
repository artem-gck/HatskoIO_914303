using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UsersService.DataAccess.Entities;
using UsersService.DataAccess.Entities.Context;
using UsersService.DataAccess.Exceptions;

namespace UsersService.DataAccess
{
    public class UserAccess : IUserAccess
    {
        private readonly UsersInfoContext _usersContext;
        private readonly ILogger<UserAccess> _userAccessLogger;

        public UserAccess(UsersInfoContext usersContext, ILogger<UserAccess> userAccessLogger)
        {
            _usersContext = usersContext ?? throw new ArgumentNullException(nameof(usersContext));
            _userAccessLogger = userAccessLogger ?? throw new ArgumentNullException(nameof(userAccessLogger));
        }

        public async Task<int> AddUserInfoAsync(UserInfoEntity userInfo)
        {
            _userAccessLogger.LogDebug("Adding entity to db with name = {name}", userInfo.Name);

            var userInfoEntity = _usersContext.UsersInfo.Add(userInfo);

            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Entity.Id;
        }

        public async Task<int> DeleteUserInfoAsync(int id)
        {
            _userAccessLogger.LogDebug("Deleting entity from db with id = {id}", id);

            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
            {
                throw new UserInfoNotFoundException(id);
            }

            var deletedUserInfoEntity = _usersContext.UsersInfo.Remove(userInfoEntity);

            await _usersContext.SaveChangesAsync();

            return deletedUserInfoEntity.Entity.Id;
        }

        public async Task<UserInfoEntity> GetUserInfoAsync(int id)
        {
            _userAccessLogger.LogDebug("Getting entity from db with id = {id}", id);

            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
            {
                throw new UserInfoNotFoundException(id);
            }

            return userInfoEntity;
        }

        public async Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync()
        {
            _userAccessLogger.LogDebug("Getting entities from db");

            var listOfUserInfoEntities = await _usersContext.UsersInfo.ToArrayAsync();

            return listOfUserInfoEntities;
        }

        public async Task<int> UpdateUserInfoAsync(int id, UserInfoEntity userInfo)
        {
            _userAccessLogger.LogDebug("Updating entity from db with id = {id}", id);

            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
            {
                throw new UserInfoNotFoundException(id);
            }

            userInfoEntity.Name = userInfo.Name;
            userInfoEntity.Surname = userInfo.Surname;
            userInfoEntity.Patronymic = userInfo.Patronymic;
            userInfoEntity.Email = userInfo.Email;

            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Id;
        }
    }
}
