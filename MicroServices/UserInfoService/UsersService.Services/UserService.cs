using AutoMapper;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepositoty _userAccess;
        private readonly IMapper _mapper;

        public UserService(IUserRepositoty userAccess, IMapper mapper)
        {
            _userAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> AddUserAsync(UserDto userDto)
            => await _userAccess.AddUserAsync(_mapper.Map<UserEntity>(userDto));

        public async Task DeleteUserAsync(Guid id)
            => await _userAccess.DeleteUserAsync(id);

        public async Task<UserDto> GetUserAsync(Guid id)
            => _mapper.Map<UserDto>(await _userAccess.GetUserAsync(id));

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
            => (await _userAccess.GetUsersAsync()).Select(us => _mapper.Map<UserDto>(us));

        public async Task UpdateUserAsync(Guid id, UserDto userDto)
            => await _userAccess.UpdateUserAsync(id, _mapper.Map<UserEntity>(userDto));
    }
}
