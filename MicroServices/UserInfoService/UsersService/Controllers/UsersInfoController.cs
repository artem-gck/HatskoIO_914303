using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UsersService.Services;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.Controllers
{
    /// <summary>
    /// Controller for users info.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiController]
    [Route("users")]
    public class UsersInfoController : Controller
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersInfoController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentNullException">
        /// userService
        /// or
        /// mapper
        /// </exception>
        public UsersInfoController(IUserService userService, IMapper mapper)
        {
            _userService = userService is not null ? userService : throw new ArgumentNullException(nameof(userService));
            _mapper = mapper is not null ? mapper : throw new ArgumentNullException(nameof(mapper));
        }

        // GET users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetAllAsync()
            => Ok((await _userService.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoViewModel>(us)));

        // GET users/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoViewModel>> GetAsync(int id)
        {
            var user = _mapper.Map<UserInfoViewModel>(await _userService.GetUserInfoAsync(id));

            return Ok(user);
        }

        // DELETE users/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userService.DeleteUserInfoAsync(id);
            
            return Ok(result);
        }

        // POST users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddAsync(UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.AddUserInfoAsync(_mapper.Map<UserInfoDto>(userInfoViewModel));

            return Created($"users/{result}", result);
        }

        // PUT users/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync(int id, UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.UpdateUserInfoAsync(id, _mapper.Map<UserInfoDto>(userInfoViewModel));
            
            return Ok(result);
        }
    }
}
