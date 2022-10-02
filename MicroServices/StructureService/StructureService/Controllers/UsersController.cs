using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureService.ViewModels;

namespace StructureService.Controllers
{
    [Route("users")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _controllerMapper;

        public UsersController(IUserService userService, IMapper controllerMapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
        }

        /// <summary>
        /// Gets the DepartmentUnitViewModel by user id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DepartmentUnitViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /users/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentUnitViewModel>> Get(Guid id)
        {
            var departmentUnitViewModel = _controllerMapper.Map<DepartmentUnitViewModel>(await _userService.GetAsync(id));

            return Ok(departmentUnitViewModel);
        }
    }
}
