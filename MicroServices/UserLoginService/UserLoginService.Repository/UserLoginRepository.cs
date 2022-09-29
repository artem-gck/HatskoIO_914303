using Microsoft.EntityFrameworkCore;
using UserLoginService.Domain.Entities;
using UserLoginService.Domain.Exceptions;
using UserLoginService.Repository.Context;

namespace UserLoginService.Repository
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly UserLoginContext _userLoginContext;

        public UserLoginRepository(UserLoginContext userLoginContext)
        {
            _userLoginContext = userLoginContext is not null ? userLoginContext : throw new ArgumentNullException(nameof(userLoginContext));
        }

        public async Task<IEnumerable<UserLoginEntity>> GetAllUsersLoginAsync()
            => await _userLoginContext.UserLogin.ToListAsync();

        public async Task<UserLoginEntity> GetUserLoginAsync(int id)
        {
            var userLoginEntity = await _userLoginContext.UserLogin.FirstOrDefaultAsync(us => us.Id == id);

            if (userLoginEntity is null)
                throw new UserLoginNotFoundException(id);

            return userLoginEntity;
        }

        public Task<int> InsertUserLoginAsync(UserLoginEntity userLoginEntity)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveUserLoginAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateUserLoginAsync(int id, UserLoginEntity userLoginEntity)
        {
            throw new NotImplementedException();
        }
    }
}
