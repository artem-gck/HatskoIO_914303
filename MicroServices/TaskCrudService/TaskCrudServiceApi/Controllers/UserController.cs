using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskCrudService.Ports.Output;
using TaskCrudService.ViewModels;

namespace TaskCrudService.Controllers
{
    [Route("users")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List of TaskViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /users/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetAll(Guid id)
        {
            var listOfTaskViewModel = _mapper.Map<IEnumerable<TaskViewModel>>(await _userService.GetAllAsync(id));

            return Ok(listOfTaskViewModel);
        }
    }
}
