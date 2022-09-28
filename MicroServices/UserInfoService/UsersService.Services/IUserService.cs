using UsersService.Services.Dto;

namespace UsersService.Services
{
    /// <summary>
    /// Interface for service with bisness logic with user info.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the users information asynchronous.
        /// </summary>
        /// <returns>List of user info.</returns>
        public Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync();

        /// <summary>
        /// Gets the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>User info by id.</returns>
        public Task<UserInfoDto> GetUserInfoAsync(int id);

        /// <summary>
        /// Deletes the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id of deleted user info.</returns>
        public Task<int> DeleteUserInfoAsync(int id);

        /// <summary>
        /// Adds the user information asynchronous.
        /// </summary>
        /// <param name="userInfoDto">The user information dto.</param>
        /// <returns>id of added user info.</returns>
        public Task<int> AddUserInfoAsync(UserInfoDto userInfoDto);

        /// <summary>
        /// Updates the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userInfoDto">The user information dto.</param>
        /// <returns>id of updated user info.</returns>
        public Task<int> UpdateUserInfoAsync(int id, UserInfoDto userInfoDto);
    }
}
