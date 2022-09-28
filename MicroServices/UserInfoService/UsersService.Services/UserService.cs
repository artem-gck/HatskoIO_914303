using AutoMapper;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;

        public UserService(IUserAccess userAccess, IMapper mapper)
         => (_userAccess, _mapper) = (userAccess, mapper);

        public async Task<int> AddUserInfoAsync(UserInfoDto userInfoDto)
            => await _userAccess.AddUserInfoAsync(_mapper.Map<UserInfoEntity>(userInfoDto));

        public async Task<int> DeleteUserInfoAsync(int id)
            => await _userAccess.DeleteUserInfoAsync(id);

        public async Task<UserInfoDto> GetUserInfoAsync(int id)
            => _mapper.Map<UserInfoDto>(await _userAccess.GetUserInfoAsync(id));

        public async Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync()
            => (await _userAccess.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoDto>(us));

        public async Task<int> UpdateUserInfoAsync(int id, UserInfoDto userInfoDto)
            => await _userAccess.UpdateUserInfoAsync(id, _mapper.Map<UserInfoEntity>(userInfoDto));
    }
}
