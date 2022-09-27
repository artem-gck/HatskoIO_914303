using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.DataAccess.Entities;
using UsersService.DataAccess.Entities.Context;

namespace UsersService.DataAccess
{
    public class UserAccess : IUserAccess
    {
        private readonly UsersInfoContext _usersContext;

        public UserAccess(UsersInfoContext usersContext)
            => _usersContext = usersContext;

        public async Task<int> AddUserInfoAsync(UserInfoEntity userInfo)
        {
            var positionEntity = await GetPosition(userInfo.Position.Name);

            userInfo.Position = positionEntity;

            var userInfoEntity = _usersContext.UsersInfo.Add(userInfo);

            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Entity.Id;
        }

        public async Task<int> DeleteUserInfoAsync(int id)
        {
            var userInfoEntity = await _usersContext.UsersInfo.Include(us => us.Position)
                                                              .FirstAsync(us => us.Id == id);

            var deletedUserInfoEntity = _usersContext.UsersInfo.Remove(userInfoEntity);

            await _usersContext.SaveChangesAsync();

            return deletedUserInfoEntity.Entity.Id;
        }

        public async Task<UserInfoEntity> GetUserInfoAsync(int id)
            => await _usersContext.UsersInfo.Include(us => us.Position)
                                            .FirstAsync(us => us.Id == id);

        public async Task<IEnumerable<UserInfoEntity>> GetUsersInfoAsync()
            => await _usersContext.UsersInfo.Include(us => us.Position)
                                            .ToArrayAsync();

        public async Task<int> UpdateUserInfoAsync(UserInfoEntity userInfo)
        {
            var userInfoEntity = await _usersContext.UsersInfo.Include(us => us.Position)
                                                              .FirstAsync(us => us.Id == userInfo.Id);

            var positionEntity = await GetPosition(userInfo.Position.Name);

            userInfoEntity.Name = userInfo.Name;
            userInfoEntity.Surname = userInfo.Surname;
            userInfoEntity.Patronymic = userInfo.Patronymic;
            userInfoEntity.Email = userInfo.Email;
            userInfoEntity.Position = positionEntity;

            await _usersContext.SaveChangesAsync();

            return userInfoEntity.Id;
        }

        private async Task<PositionEntity> GetPosition(string position)
        {
            var positionEntity = await _usersContext.Positions.FirstOrDefaultAsync(pos => pos.Name == position);

            if (positionEntity is null)
            {
                positionEntity = new PositionEntity()
                {
                    Name = position
                };

                var roleEntity = _usersContext.Positions.Add(positionEntity);

                await _usersContext.SaveChangesAsync();

                return roleEntity.Entity;
            }

            return positionEntity;
        }
    }
}
