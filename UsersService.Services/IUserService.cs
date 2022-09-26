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
        public IEnumerable<User> GetUsers();
        public User GetUser(int id);
        public int DeleteUser(int id);
        public int AddUser(User user);
        public int UpdateUser(User user);
    }
}
