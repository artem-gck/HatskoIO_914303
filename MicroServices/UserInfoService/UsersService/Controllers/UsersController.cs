using Microsoft.AspNetCore.Mvc;
using UsersService.Services.Models;
using UsersService.Services;
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
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
            => Json((await _userService.GetUsers()).Select(us => MapUser(us)));

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> Get(int id)
        {
            UserViewModel user;

            try
            {
                user = MapUser(await _userService.GetUser(id));

                return user;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int result;

            try
            {
                result = await _userService.DeleteUser(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel user)
        {
            int result;

            try
            {
                result = await _userService.AddUser(MapUserViewModel(user));

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserViewModel user)
        {
            int result;

            try
            {
                result = await _userService.UpdateUser(MapUserViewModel(user));

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private UserViewModel MapUser(User user)
            => new UserViewModel()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Role = user.Role,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                AccessToken = user.AccessToken,
                RefreshToken = user.RefreshToken
            };

        private User MapUserViewModel(UserViewModel userViewModel)
           => new User()
           {
               Id = userViewModel.Id,
               Login = userViewModel.Login,
               Password = userViewModel.Password,
               Role = userViewModel.Role,
               Name = userViewModel.Name,
               Surname = userViewModel.Surname,
               Patronymic = userViewModel.Patronymic,
               AccessToken = userViewModel.AccessToken,
               RefreshToken = userViewModel.RefreshToken
           };
    }
}
