using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.DataAccess;
using UsersService.DataAccess.dto;
using UsersService.Services.Models;

namespace UsersService.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserAccess _userAccess;

        public UserService(IUserAccess userAccess)
            => _userAccess = userAccess;

        public async Task<int> AddUser(User user)
            => await _userAccess.AddUser(MapUser(user));

        public async Task<int> DeleteUser(int id)
            => await _userAccess.DeleteUser(id);

        public async Task<User> GetUser(int id)
            => MapUserDto(await _userAccess.GetUser(id));

        public async Task<IEnumerable<User>> GetUsers()
            => (await _userAccess.GetUsers()).Select(us => MapUserDto(us));

        public async Task<int> UpdateUser(User user)
            => await _userAccess.UpdateUser(MapUser(user));

        private User MapUserDto(UserDto userDto)
        {
            var user = new User()
            {
                Id = userDto.Id,
                Login = userDto.Login,
                Password = userDto.Password,
                Role = userDto.Role.Name,
                Name = userDto.UserInfo.Name,
                Surname = userDto.UserInfo.Surname,
                Patronymic = userDto.UserInfo.Patronymic,
                AccessToken = userDto.Tokens.AccessToken,
                RefreshToken = userDto.Tokens.RefreshToken
            };

            return user;
        }

        private UserDto MapUser(User user)
        {
            var userInfoDto = new UserInfoDto()
            {
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic
            };

            var roleDto = new RoleDto()
            {
                Name = user.Role
            };

            var tokensDto = new TokensDto()
            {
                AccessToken = user.AccessToken,
                RefreshToken = user.RefreshToken
            };

            var userDto = new UserDto()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Role = roleDto,
                UserInfo = userInfoDto,
                Tokens = tokensDto
            };

            return userDto;
        }
    }
}
