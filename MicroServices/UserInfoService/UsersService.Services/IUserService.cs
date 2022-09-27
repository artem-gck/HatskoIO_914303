using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Services.Dto;

namespace UsersService.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync();
        public Task<UserInfoDto> GetUserInfoAsync(int id);
        public Task<int> DeleteUserInfoAsync(int id);
        public Task<int> AddUserInfoAsync(UserInfoDto userInfoDto);
        public Task<int> UpdateUserInfoAsync(UserInfoDto userInfoDto);
    }
}
