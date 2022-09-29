using AutoMapper;
using Microsoft.Extensions.Logging;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services
{
    /// <summary>
    /// Service with bisness logic with user info.
    /// </summary>
    /// <seealso cref="UsersService.Services.IUserService" />
    public class UserService : IUserService
    {
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _userServiceLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userAccess">The user access.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentNullException">
        /// userAccess
        /// or
        /// mapper
        /// </exception>
        public UserService(IUserAccess userAccess, IMapper mapper, ILogger<UserService> userServiceLogger)
        {
            _userAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userServiceLogger = userServiceLogger ?? throw new ArgumentNullException(nameof(userServiceLogger));
        }

        /// <summary>
        /// Adds the user information asynchronous.
        /// </summary>
        /// <param name="userInfoDto">The user information dto.</param>
        /// <returns>
        /// id of added user info.
        /// </returns>
        public async Task<int> AddUserInfoAsync(UserInfoDto userInfoDto)
        {
            _userServiceLogger.LogInformation("Start adding user info to access level");

            var result = await _userAccess.AddUserInfoAsync(_mapper.Map<UserInfoEntity>(userInfoDto));

            _userServiceLogger.LogInformation("Start adding user info to access level");

            return result;
        }

        /// <summary>
        /// Deletes the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// id of deleted user info.
        /// </returns>
        public async Task<int> DeleteUserInfoAsync(int id)
        {
            _userServiceLogger.LogInformation("Start deleting user info from access level");

            var result = await _userAccess.DeleteUserInfoAsync(id);

            _userServiceLogger.LogInformation("Success adding user info from access level");

            return result;
        }

        /// <summary>
        /// Gets the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// User info by id.
        /// </returns>
        public async Task<UserInfoDto> GetUserInfoAsync(int id)
        {
            _userServiceLogger.LogInformation("Start getting user info from access level");

            var userInfoDto = _mapper.Map<UserInfoDto>(await _userAccess.GetUserInfoAsync(id));

            _userServiceLogger.LogInformation("Success getting user info from access level");

            return userInfoDto;
        }

        /// <summary>
        /// Gets the users information asynchronous.
        /// </summary>
        /// <returns>
        /// List of user info.
        /// </returns>
        public async Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync()
        {
            _userServiceLogger.LogInformation("Start getting list of user info from access level");

            var listOfUsersDto = (await _userAccess.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoDto>(us));

            _userServiceLogger.LogInformation("Success getting list of user info from access level");

            return listOfUsersDto;
        }

        /// <summary>
        /// Updates the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userInfoDto">The user information dto.</param>
        /// <returns>
        /// id of updated user info.
        /// </returns>
        public async Task<int> UpdateUserInfoAsync(int id, UserInfoDto userInfoDto)
        {
            _userServiceLogger.LogInformation("Start updating user info at access level");

            var result = await _userAccess.UpdateUserInfoAsync(id, _mapper.Map<UserInfoEntity>(userInfoDto));

            _userServiceLogger.LogInformation("Success updating user info at access level");

            return result;
        }
    }
}
