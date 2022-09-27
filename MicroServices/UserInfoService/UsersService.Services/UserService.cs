using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserAccess _userAccess;

        public UserService(IUserAccess userAccess)
            => _userAccess = userAccess;

        public async Task<int> AddUserInfoAsync(UserInfoDto userInfoDto)
            => await _userAccess.AddUserInfoAsync(MapUserInfoDto(userInfoDto));

        public async Task<int> DeleteUserInfoAsync(int id)
            => await _userAccess.DeleteUserInfoAsync(id);

        public async Task<UserInfoDto> GetUserInfoAsync(int id)
            => MapUserInfoEntity(await _userAccess.GetUserInfoAsync(id));

        public async Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync()
            => (await _userAccess.GetUsersInfoAsync()).Select(us => MapUserInfoEntity(us));

        public async Task<int> UpdateUserInfoAsync(UserInfoDto userInfoDto)
            => await _userAccess.UpdateUserInfoAsync(MapUserInfoDto(userInfoDto));

        private UserInfoDto MapUserInfoEntity(UserInfoEntity userInfoEntity)
        {
            var userInfoDto = new UserInfoDto()
            {
                Id = userInfoEntity.Id,
                Name = userInfoEntity.Name,
                Surname = userInfoEntity.Surname,
                Patronymic = userInfoEntity.Patronymic,
                Email = userInfoEntity.Email,
                Position = userInfoEntity.Position.Name
            };

            return userInfoDto;
        }

        private UserInfoEntity MapUserInfoDto(UserInfoDto userInfoDto)
        {
            var positionEntity = new PositionEntity()
            {
                Name = userInfoDto.Position
            };

            var userInfoEntity = new UserInfoEntity()
            {
                Name = userInfoDto.Name,
                Surname = userInfoDto.Surname,
                Patronymic = userInfoDto.Patronymic,
                Email = userInfoDto.Email,
                Position = positionEntity
            };

            return userInfoEntity;
        }
    }
}
