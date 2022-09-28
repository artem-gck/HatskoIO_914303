using UsersService.DataAccess.Entities;

namespace UsersService.DataAccess
{
    /// <summary>
    /// Interface for access to database.
    /// </summary>
    public interface IUserAccess
    {
        /// <summary>
        /// Gets the users information asynchronous.
        /// </summary>
        /// <returns>List of user info.</returns>
        public Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync();

        /// <summary>
        /// Gets the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>User info by id.</returns>
        public Task<UserInfoEntity> GetUserInfoAsync(int id);

        /// <summary>
        /// Deletes the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id of deleted user info.</returns>
        public Task<int> DeleteUserInfoAsync(int id);

        /// <summary>
        /// Adds the user information asynchronous.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns>id of added user info.</returns>
        public Task<int> AddUserInfoAsync(UserInfoEntity userInfo);

        /// <summary>
        /// Updates the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userInfo">The user information.</param>
        /// <returns>id of updated user info.</returns>
        public Task<int> UpdateUserInfoAsync(int id, UserInfoEntity userInfo);
    }
}
