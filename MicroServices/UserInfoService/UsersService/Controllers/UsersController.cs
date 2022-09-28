using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UsersService.Services;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService is not null ? userService : throw new ArgumentNullException(nameof(userService));
            _mapper = mapper is not null ? mapper : throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetAllAsync()
            => Ok((await _userService.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoViewModel>(us)));

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoViewModel>> GetAsync(int id)
        {
            var user = _mapper.Map<UserInfoViewModel>(await _userService.GetUserInfoAsync(id));

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userService.DeleteUserInfoAsync(id);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.AddUserInfoAsync(_mapper.Map<UserInfoDto>(userInfoViewModel));

            return Created($"users/{result}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.UpdateUserInfoAsync(id, _mapper.Map<UserInfoDto>(userInfoViewModel));
            
            return Ok(result);
        }
    }
}
