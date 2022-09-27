using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Services.Models;

namespace UsersService.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetUser(int id);
        public Task<int> DeleteUser(int id);
        public Task<int> AddUser(User user);
        public Task<int> UpdateUser(User user);
    }
}
