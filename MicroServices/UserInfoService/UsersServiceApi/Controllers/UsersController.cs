using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersService.Services;
using UsersService.Services.Dto;
using UsersServiceApi.VewModels;

namespace UsersServiceApi.Controllers
{
    [Route("api/users")]
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
        /// <response code="500">Interal server error</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserResponce>>> Get()
        {
            var listOfUserInfo = (await _userService.GetUsersAsync()).Select(us => _mapper.Map<UserResponce>(us));
            var listOfId = string.Join(", ", listOfUserInfo.Select(us => us.Id.ToString()));

            _userLogger.LogDebug("Taken list of id of user info: {listOfId}", listOfId);

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
        /// <response code="404">No userInfo with this id</response>
        /// <response code="500">Interal server error</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponce>> Get(Guid id)
        {
            _userLogger.LogDebug("Getting user info from service");

            var user = _mapper.Map<UserResponce>(await _userService.GetUserAsync(id));

            return Ok(user);
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
        /// <response code="404">No userInfo with this id</response>
        /// <response code="500">Interal server error</response>
        [Authorize]
        [HttpGet("~/api/departments/{id}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponce>> GetByDepartmentId(Guid id)
        {
            _userLogger.LogDebug("Getting user info from service by department id = {id}", id);

            var users = _mapper.Map<IEnumerable<UserResponce>>(await _userService.GetUsersByDepartmentId(id));

            return Ok(users);
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
        /// <response code="404">No userInfo with this id</response>
        /// <response code="500">Interal server error</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _userLogger.LogDebug("Deleting user info from service with id = {id}", id);

            await _userService.DeleteUserAsync(id);

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
        ///        "id": "e7bc77c8-7075-4d89-b569-051d8b28676c",
        ///        "name": "Artem",
        ///        "surname": "Gatsko",
        ///        "patronymic": "Aliaksandrovich",
        ///        "email": "qwe@gmail.com",
        ///        "departmentId": "e7bc77c8-7075-4d89-b569-051d8b28676c",
        ///        "positionId": "e7bc77c8-7075-4d89-b569-051d8b28676c"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">UserInfo was created</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="500">Interal server error</response>
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Post(AddUserRequest userInfoViewModel)
        //{
        //    _userLogger.LogDebug("Adding user info to service with name = {name}", userInfoViewModel.Name);

        //    if (!ModelState.IsValid)
        //    {
        //        _userLogger.LogWarning("Invalid model state, count of errors = {ErrorCount}", ModelState.ErrorCount);

        //        return BadRequest(ModelState);
        //    }

        //    var result = await _userService.AddUserAsync(_mapper.Map<UserDto>(userInfoViewModel));

        //    _userLogger.LogDebug("Id of added user indo is {id}", result);

        //    return Created($"users/{result}", result);
        //}

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
        ///        "id": "e7bc77c8-7075-4d89-b569-051d8b28676c",
        ///        "name": "Artem",
        ///        "surname": "Gatsko",
        ///        "patronymic": "Aliaksandrovich",
        ///        "email": "qwe@gmail.com",
        ///        "departmentId": "e7bc77c8-7075-4d89-b569-051d8b28676c",
        ///        "positionId": "e7bc77c8-7075-4d89-b569-051d8b28676c"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">UserInfo was updated</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="404">No userInfo with this id</response>
        /// <response code="500">Interal server error</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateUserRequest userInfoViewModel)
        {
            _userLogger.LogDebug("Updating user info at service with id = {id}", userInfoViewModel.Id);

            if (!ModelState.IsValid)
            {
                _userLogger.LogWarning("Invalid model state, count of errors = {ErrorCount}", ModelState.ErrorCount);

                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(id, _mapper.Map<UserDto>(userInfoViewModel));

            return NoContent();
        }
    }
}
