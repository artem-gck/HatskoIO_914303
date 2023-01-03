using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UsersService.DataAccess.Entities;
using UsersService.DataAccess.Entities.Context;
using UsersService.DataAccess.Exceptions;

namespace UsersService.DataAccess
{
    public class UserRepository : IUserRepositoty
    {
        private readonly UsersContext _usersContext;
        private readonly ILogger<UserRepository> _userAccessLogger;

        public UserRepository(UsersContext usersContext, ILogger<UserRepository> userAccessLogger)
        {
            _usersContext = usersContext ?? throw new ArgumentNullException(nameof(usersContext));
            _userAccessLogger = userAccessLogger ?? throw new ArgumentNullException(nameof(userAccessLogger));
        }

        public async Task<Guid> AddUserAsync(UserEntity user)
        {
            _userAccessLogger.LogDebug("Adding entity to db with name = {name}", user.Name);

            var userInfoEntity = _usersContext.Users.Add(user);
            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Entity.Id;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            _userAccessLogger.LogDebug("Deleting entity from db with id = {id}", id);

            var userEntity = await _usersContext.Users.FirstOrDefaultAsync(us => us.Id == id);

            if (userEntity is null)
            {
                throw new UserInfoNotFoundException(id);
            }

            var deletedUserInfoEntity = _usersContext.Users.Remove(userEntity);
            await _usersContext.SaveChangesAsync();
        }

        public async Task<UserEntity> GetUserAsync(Guid id)
        {
            _userAccessLogger.LogDebug("Getting entity from db with id = {id}", id);

            var userInfoEntity = await _usersContext.Users.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
                throw new UserInfoNotFoundException(id);

            return userInfoEntity;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            _userAccessLogger.LogDebug("Getting entities from db");

            var listOfUserInfoEntities = await _usersContext.Users.ToArrayAsync();

            return listOfUserInfoEntities;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync(int page, int count)
        {
            _userAccessLogger.LogDebug("Getting entities from db");

            var listOfUserInfoEntities = await _usersContext.Users.Skip(page * count)
                                                                  .Take(count)
                                                                  .ToArrayAsync();

            return listOfUserInfoEntities;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersByDepartmentId(Guid departmentId)
        {
            _userAccessLogger.LogDebug($"Getting entities from db by department id = {departmentId}");

            var listOfUserInfoEntities = await _usersContext.Users.Where(us => us.DepartmentId == departmentId)
                                                                  .ToArrayAsync();

            return listOfUserInfoEntities;
        }

        public async Task UpdateUserAsync(Guid id, UserEntity user)
        {
            _userAccessLogger.LogDebug("Updating entity from db with id = {id}", id);

            var userInfoEntity = await _usersContext.Users.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
                throw new UserInfoNotFoundException(id);

            userInfoEntity.Name = user.Name;
            userInfoEntity.Surname = user.Surname;
            userInfoEntity.Patronymic = user.Patronymic;

            await _usersContext.SaveChangesAsync();
        }

        public async Task UpdatePositionAndDepartmentOfUserAsync(Guid id, UserEntity user)
        {
            var userInfoEntity = await _usersContext.Users.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
                throw new UserInfoNotFoundException(id);

            userInfoEntity.DepartmentId = user.DepartmentId;
            userInfoEntity.PositionId = user.PositionId;

            await _usersContext.SaveChangesAsync();
        }

        public async Task UpdateEmailOfUserAsync(Guid id, UserEntity user)
        {
            var userInfoEntity = await _usersContext.Users.FirstOrDefaultAsync(us => us.Id == id);

            if (userInfoEntity is null)
                throw new UserInfoNotFoundException(id);

            userInfoEntity.Email = user.Email;

            await _usersContext.SaveChangesAsync();
        }
    }
}
