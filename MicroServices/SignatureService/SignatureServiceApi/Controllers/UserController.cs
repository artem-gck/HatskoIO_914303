using Microsoft.AspNetCore.Mvc;
using SignatureService.Services.Interfaces;

namespace SignatureServiceApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(Guid id)
        {
            return Ok(await _userService.AddUserAsync(id));
        }
    }
}
