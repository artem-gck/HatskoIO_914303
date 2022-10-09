using UsersService.Services.Dto;

namespace UsersService.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserInfoDto>> GetUsersInfoAsync();
        public Task<UserInfoDto> GetUserInfoAsync(Guid id);
        public Task DeleteUserInfoAsync(Guid id);
        public Task<Guid> AddUserInfoAsync(UserInfoDto userInfoDto);
        public Task UpdateUserInfoAsync(Guid id, UserInfoDto userInfoDto);
    }
}
