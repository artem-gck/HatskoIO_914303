using AutoMapper;
using Microsoft.Extensions.Logging;
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
        {
            _userAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> AddUserInfoAsync(UserInfoDto userInfoDto)
        {
            var result = await _userAccess.AddUserInfoAsync(_mapper.Map<UserInfoEntity>(userInfoDto));

            return result;
        }

        public async Task<int> DeleteUserInfoAsync(int id)
        {
            var result = await _userAccess.DeleteUserInfoAsync(id);

            return result;
        }

        public async Task<UserInfoDto> GetUserInfoAsync(int id)
        {
            var userInfoDto = _mapper.Map<UserInfoDto>(await _userAccess.GetUserInfoAsync(id));

            return userInfoDto;
        }

        public async Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync()
        {
            var listOfUsersDto = (await _userAccess.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoDto>(us));

            return listOfUsersDto;
        }

        public async Task<int> UpdateUserInfoAsync(int id, UserInfoDto userInfoDto)
        {
            var result = await _userAccess.UpdateUserInfoAsync(id, _mapper.Map<UserInfoEntity>(userInfoDto));

            return result;
        }
    }
}
