using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.DataAccess.dto;
using UsersService.DataAccess.dto.context;

namespace UsersService.DataAccess.Implementation
{
    public class UserAccess : IUserAccess
    {
        private readonly UsersContext _usersContext;

        public UserAccess(UsersContext usersContext)
            => _usersContext = usersContext;

        public async Task<int> AddUser(UserDto user)
        {
            var entity = await _usersContext.Users.AddAsync(user);

            await _usersContext.SaveChangesAsync();

            return entity.Entity.Id;
        }

        public async Task<int> DeleteUser(int id)
        {
            var entity = await _usersContext.Users.Include(us => us.UserInfo)
                                                  .Include(us => us.Tokens)
                                                  .Include(us => us.Role)
                                                  .FirstOrDefaultAsync(us => us.Id == id);

            if (entity is null)
                throw new ArgumentNullException("id", "No user with this id");

            var deletedEntity = _usersContext.Users.Remove(entity);

            await _usersContext.SaveChangesAsync();

            return deletedEntity.Entity.Id;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _usersContext.Users.Include(us => us.UserInfo)
                                                .Include(us => us.Tokens)
                                                .Include(us => us.Role)
                                                .FirstOrDefaultAsync(us => us.Id == id);

            if (user is null)
                throw new ArgumentNullException("entity", "No user with this id");

            return user;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
            => await _usersContext.Users.Include(us => us.UserInfo)
                                        .Include(us => us.Tokens)
                                        .Include(us => us.Role)
                                        .ToArrayAsync();

        public async Task<int> UpdateUser(UserDto user)
        {
            var userDb = await _usersContext.Users.Include(us => us.UserInfo)
                                                  .Include(us => us.Tokens)
                                                  .Include(us => us.Role)
                                                  .FirstOrDefaultAsync(us => us.Id == user.Id);

            if (user is null)
                throw new ArgumentNullException("entity", "No user with this id");

            userDb.Login = user.Login;
            userDb.Password = user.Password;
            userDb.Role.Name = user.Role.Name;
            userDb.UserInfo.Name = user.UserInfo.Name;
            userDb.UserInfo.Surname = user.UserInfo.Surname;
            userDb.UserInfo.Patronymic = user.UserInfo.Patronymic;

            await _usersContext.SaveChangesAsync();

            return userDb.Id;
        }
    }
}
