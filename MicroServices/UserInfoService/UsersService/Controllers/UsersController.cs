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
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _userLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentNullException">
        /// userService
        /// or
        /// mapper
        /// </exception>
        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> userLogger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userLogger = userLogger ?? throw new ArgumentNullException(nameof(userLogger));
        }

        // GET users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetAll()
        {
            _userLogger.LogInformation("Start getting list of user info from service");

            var listOfUserInfo = (await _userService.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoViewModel>(us));

            _userLogger.LogInformation("Success getting list of user info from service");

            return Ok(listOfUserInfo);
        }

        // GET users/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoViewModel>> Get(int id)
        {
            _userLogger.LogInformation("Start getting user info from service");

            var user = _mapper.Map<UserInfoViewModel>(await _userService.GetUserInfoAsync(id));

            _userLogger.LogInformation("Success getting user info from service");

            return Ok(user);
        }

        // DELETE users/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            _userLogger.LogInformation("Start deleting user info from service");

            var result = await _userService.DeleteUserInfoAsync(id);

            _userLogger.LogInformation("Success deleting user info from service");

            return NoContent();
        }

        // POST users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(UserInfoViewModel userInfoViewModel)
        {
            _userLogger.LogInformation("Start adding user info to service");

            if (!ModelState.IsValid)
            {
                _userLogger.LogWarning("Invalid model state, count of errors = {ErrorCount}", ModelState.ErrorCount);

                return BadRequest(ModelState);
            }

            var result = await _userService.AddUserInfoAsync(_mapper.Map<UserInfoDto>(userInfoViewModel));

            _userLogger.LogInformation("Success adding user info to service");

            return Created($"users/{result}", result);
        }

        // PUT users/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(int id, UserInfoViewModel userInfoViewModel)
        {
            _userLogger.LogInformation("Start updating user info at service");

            if (!ModelState.IsValid)
            {
                _userLogger.LogWarning("Invalid model state, count of errors = {ErrorCount}", ModelState.ErrorCount);

                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserInfoAsync(id, _mapper.Map<UserInfoDto>(userInfoViewModel));

            _userLogger.LogInformation("Success updating user info at service");

            return NoContent();
        }
    }
}
