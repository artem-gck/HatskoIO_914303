using AutoMapper;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserInfoRepositoty _userAccess;
        private readonly IMapper _mapper;

        public UserService(IUserInfoRepositoty userAccess, IMapper mapper)
        {
            _userAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> AddUserInfoAsync(UserInfoDto userInfoDto)
            => await _userAccess.AddUserInfoAsync(_mapper.Map<UserEntity>(userInfoDto));

        public async Task DeleteUserInfoAsync(Guid id)
            => await _userAccess.DeleteUserInfoAsync(id);

        public async Task<UserInfoDto> GetUserInfoAsync(Guid id)
            => _mapper.Map<UserInfoDto>(await _userAccess.GetUserInfoAsync(id));

        public async Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync()
            => (await _userAccess.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoDto>(us));

        public async Task UpdateUserInfoAsync(Guid id, UserInfoDto userInfoDto)
            => await _userAccess.UpdateUserInfoAsync(id, _mapper.Map<UserEntity>(userInfoDto));
    }
}
