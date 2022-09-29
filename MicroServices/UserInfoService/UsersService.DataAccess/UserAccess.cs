using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccess"/> class.
        /// </summary>
        /// <param name="usersContext">The users context.</param>
        /// <exception cref="System.ArgumentNullException">usersContext</exception>
        public UserAccess(UsersInfoContext usersContext)
            => _usersContext = usersContext ?? throw new ArgumentNullException(nameof(usersContext));

        /// <summary>
        /// Adds the user information asynchronous.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns>
        /// id of added user info.
        /// </returns>
        public async Task<int> AddUserInfoAsync(UserInfoEntity userInfo)
        {
            var userInfoEntity = _usersContext.UsersInfo.Add(userInfo);

            await _usersContext.SaveChangesAsync();

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
            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
                throw new UserInfoNotFoundException(id);

            var deletedUserInfoEntity = _usersContext.UsersInfo.Remove(userInfoEntity);

            await _usersContext.SaveChangesAsync();

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
            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
                throw new UserInfoNotFoundException(id);

            return userInfoEntity;
        }

        /// <summary>
        /// Gets the users information asynchronous.
        /// </summary>
        /// <returns>
        /// List of user info.
        /// </returns>
        public async Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync()
            => await _usersContext.UsersInfo.ToArrayAsync();

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
            var userInfoEntity = await _usersContext.UsersInfo.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
                throw new UserInfoNotFoundException(id);

            userInfoEntity.Name = userInfo.Name;
            userInfoEntity.Surname = userInfo.Surname;
            userInfoEntity.Patronymic = userInfo.Patronymic;
            userInfoEntity.Email = userInfo.Email;

            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Id;
        }
    }
}
