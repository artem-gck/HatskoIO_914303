using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.DataAccess.dto;

namespace UsersService.DataAccess
{
    public interface IUserAccess
    {
        public Task<IEnumerable<UserDto>> GetUsers();
        public Task<UserDto> GetUser(int id);
        public Task<int> DeleteUser(int id);
        public Task<int> AddUser(UserDto user);
        public Task<int> UpdateUser(UserDto user);
    }
}
