using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UsersService.Services;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.Controllers
{
    [Route("users")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _userLogger;

        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> userLogger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userLogger = userLogger ?? throw new ArgumentNullException(nameof(userLogger));
        }

        /// <summary>
        /// Get all user info
        /// </summary>
        /// <returns>List of user info model</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET /users
        ///
        /// </remarks>
        /// <response code="200">List of user info was getting</response>
        /// <response code="400">Problem of server side</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetAll()
        {
            _userLogger.LogInformation("Start getting list of user info from service");

            var listOfUserInfo = (await _userService.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoViewModel>(us));

            _userLogger.LogInformation("Success getting list of user info from service");

            return Ok(listOfUserInfo);
        }

        /// <summary>
        /// Get user info by id
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>User info model</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET /users/{id}
        ///
        /// </remarks>
        /// <response code="200">User info was getting</response>
        /// <response code="400">Problem of server side</response>
        /// <response code="404">No userInfo with this id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserInfoViewModel>> Get(int id)
        {
            _userLogger.LogInformation("Start getting user info from service");

            var user = _mapper.Map<UserInfoViewModel>(await _userService.GetUserInfoAsync(id));

            _userLogger.LogInformation("Success getting user info from service");

            return Ok(user);
        }

        /// <summary>
        /// Delete user info by id
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /users/{id}
        ///
        /// </remarks>
        /// <response code="204">UserInfo was deleted</response>
        /// <response code="400">Problem of server side</response>
        /// <response code="404">No userInfo with this id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _userLogger.LogInformation("Start deleting user info from service");

            var result = await _userService.DeleteUserInfoAsync(id);

            _userLogger.LogInformation("Success deleting user info from service");

            return NoContent();
        }

        /// <summary>
        /// Add the specified user info
        /// </summary>
        /// <param name="userInfoViewModel">The user information view model</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /users
        ///     {
        ///        "name": "Artem",
        ///        "surname": "Gatsko",
        ///        "patronymic": "Aliaksandrovich",
        ///        "email": "qwe@gmail.com"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">UserInfo was created</response>
        /// <response code="400">Invalid model state</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Update the specified user info
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="userInfoViewModel">The user information view model</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /users/{id}
        ///     {
        ///        "name": "Artem",
        ///        "surname": "Gatsko",
        ///        "patronymic": "Aliaksandrovich",
        ///        "email": "qwe@gmail.com"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">UserInfo was updated</response>
        /// <response code="404">No userInfo with this id</response>
        /// <response code="400">Invalid model state</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
