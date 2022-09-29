using AutoMapper;
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
        public UserService(IUserAccess userAccess, IMapper mapper)
        {
            _userAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Adds the user information asynchronous.
        /// </summary>
        /// <param name="userInfoDto">The user information dto.</param>
        /// <returns>
        /// id of added user info.
        /// </returns>
        public async Task<int> AddUserInfoAsync(UserInfoDto userInfoDto)
            => await _userAccess.AddUserInfoAsync(_mapper.Map<UserInfoEntity>(userInfoDto));

        /// <summary>
        /// Deletes the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// id of deleted user info.
        /// </returns>
        public async Task<int> DeleteUserInfoAsync(int id)
            => await _userAccess.DeleteUserInfoAsync(id);

        /// <summary>
        /// Gets the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// User info by id.
        /// </returns>
        public async Task<UserInfoDto> GetUserInfoAsync(int id)
            => _mapper.Map<UserInfoDto>(await _userAccess.GetUserInfoAsync(id));

        /// <summary>
        /// Gets the users information asynchronous.
        /// </summary>
        /// <returns>
        /// List of user info.
        /// </returns>
        public async Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync()
            => (await _userAccess.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoDto>(us));

        /// <summary>
        /// Updates the user information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userInfoDto">The user information dto.</param>
        /// <returns>
        /// id of updated user info.
        /// </returns>
        public async Task<int> UpdateUserInfoAsync(int id, UserInfoDto userInfoDto)
            => await _userAccess.UpdateUserInfoAsync(id, _mapper.Map<UserInfoEntity>(userInfoDto));
    }
}
