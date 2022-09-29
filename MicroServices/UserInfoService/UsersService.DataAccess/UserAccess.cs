using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UsersService.DataAccess.Entities;
using UsersService.DataAccess.Entities.Context;
using UsersService.DataAccess.Exceptions;

namespace UsersService.DataAccess
{
    /// <summary>
    /// Service for access to database.
    /// </summary>
    /// <seealso cref="UsersService.DataAccess.IUserAccess" />
    public class UserAccess : IUserAccess
    {
        private readonly UsersInfoContext _usersContext;
        private readonly ILogger<UserAccess> _userAccessLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccess"/> class.
        /// </summary>
        /// <param name="usersContext">The users context.</param>
        /// <exception cref="System.ArgumentNullException">usersContext</exception>
        public UserAccess(UsersInfoContext usersContext, ILogger<UserAccess> userAccessLogger)
        {
            _usersContext = usersContext ?? throw new ArgumentNullException(nameof(usersContext));
            _userAccessLogger = userAccessLogger ?? throw new ArgumentNullException(nameof(userAccessLogger));
        }

        /// <summary>
        /// Adds the user information asynchronous.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns>
        /// id of added user info.
        /// </returns>
        public async Task<int> AddUserInfoAsync(UserInfoEntity userInfo)
        {
            _userAccessLogger.LogInformation("Start adding entity to db");

            var userInfoEntity = _usersContext.UsersInfo.Add(userInfo);

            await _usersContext.SaveChangesAsync();

            _userAccessLogger.LogInformation("Success adding entity to db");

            return userInfoEntity.Entity.Id;
        }

        /// <summary>
        /// Deletes the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// id of deleted user info.
        /// </returns>
        /// <exception cref="UsersService.DataAccess.Exceptions.UserInfoNotFoundException"></exception>
        public async Task<int> DeleteUserInfoAsync(int id)
        {
            _userAccessLogger.LogInformation("Start deleting entity to db");

            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
            {
                _userAccessLogger.LogWarning("No user info with id = {id}", id);

                throw new UserInfoNotFoundException(id);
            }

            var deletedUserInfoEntity = _usersContext.UsersInfo.Remove(userInfoEntity);

            await _usersContext.SaveChangesAsync();

            _userAccessLogger.LogInformation("Success delete entity to db");

            return deletedUserInfoEntity.Entity.Id;
        }

        /// <summary>
        /// Gets the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// User info by id.
        /// </returns>
        /// <exception cref="UsersService.DataAccess.Exceptions.UserInfoNotFoundException"></exception>
        public async Task<UserInfoEntity> GetUserInfoAsync(int id)
        {
            _userAccessLogger.LogInformation("Start getting entity from db");

            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
            {
                _userAccessLogger.LogWarning("No user info with id = {id}", id);

                throw new UserInfoNotFoundException(id);
            }

            _userAccessLogger.LogInformation("Success getting entity from db");

            return userInfoEntity;
        }

        /// <summary>
        /// Gets the users information asynchronous.
        /// </summary>
        /// <returns>
        /// List of user info.
        /// </returns>
        public async Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync()
        {
            _userAccessLogger.LogInformation("Start getting entities from db");

            var listOfUserInfoEntities = await _usersContext.UsersInfo.ToArrayAsync();

            _userAccessLogger.LogInformation("Success getting entities from db");

            return listOfUserInfoEntities;
        }

        /// <summary>
        /// Updates the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userInfo">The user information.</param>
        /// <returns>
        /// id of updated user info.
        /// </returns>
        /// <exception cref="UsersService.DataAccess.Exceptions.UserInfoNotFoundException"></exception>
        public async Task<int> UpdateUserInfoAsync(int id, UserInfoEntity userInfo)
        {
            _userAccessLogger.LogInformation("Start updating entity from db");

            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
            {
                _userAccessLogger.LogWarning("No user info with id = {id}", id);

                throw new UserInfoNotFoundException(id);
            }

            userInfoEntity.Name = userInfo.Name;
            userInfoEntity.Surname = userInfo.Surname;
            userInfoEntity.Patronymic = userInfo.Patronymic;
            userInfoEntity.Email = userInfo.Email;

            await _usersContext.SaveChangesAsync();

            _userAccessLogger.LogInformation("Success updating entity from db");

            return userInfoEntity.Id;
        }
    }
}
