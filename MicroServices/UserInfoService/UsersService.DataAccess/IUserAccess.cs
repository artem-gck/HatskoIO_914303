using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.DataAccess.Entities;

namespace UsersService.DataAccess
{
    public interface IUserAccess
    {
        public Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync();
        public Task<UserInfoEntity> GetUserInfoAsync(int id);
        public Task<int> DeleteUserInfoAsync(int id);
        public Task<int> AddUserInfoAsync(UserInfoEntity userInfo);
        public Task<int> UpdateUserInfoAsync(UserInfoEntity userInfo);
    }
}
