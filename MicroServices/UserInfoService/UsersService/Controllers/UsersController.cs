using Microsoft.AspNetCore.Mvc;
using UsersService.Services;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
            => _userService = userService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetUsersInfoAsync()
            => Json((await _userService.GetUsersInfoAsync()).Select(us => MapUserInfoDto(us)));

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoViewModel>> GetUserInfoAsync(int id)
        {
            var user = MapUserInfoDto(await _userService.GetUserInfoAsync(id));

            return Json(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInfoAsync(int id)
        {
            var result = await _userService.DeleteUserInfoAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserInfoAsync(UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.AddUserInfoAsync(MapUserInfoViewModel(userInfoViewModel));

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.UpdateUserInfoAsync(MapUserInfoViewModel(userInfoViewModel));

            return Ok(result);
        }

        private UserInfoViewModel MapUserInfoDto(UserInfoDto userInfoDto)
            => new UserInfoViewModel()
            {
                Id = userInfoDto.Id,
                Name = userInfoDto.Name,
                Surname = userInfoDto.Surname,
                Patronymic = userInfoDto.Patronymic,
                Email = userInfoDto.Email,
                Position = userInfoDto.Position,
            };

        private UserInfoDto MapUserInfoViewModel(UserInfoViewModel userInfoViewModel)
           => new UserInfoDto()
           {
               Id = userInfoViewModel.Id,
               Name = userInfoViewModel.Name,
               Surname = userInfoViewModel.Surname,
               Patronymic = userInfoViewModel.Patronymic,
               Email = userInfoViewModel.Email,
               Position = userInfoViewModel.Position,
           };
    }
}
