using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureService.Domain.Entities;
using StructureService.Infrastructure.Services;
using StructureServiceApi.ViewModels;

namespace StructureServiceApi.Controllers
{
    [Route("api/department-units")]
    [Produces("application/json")]
    public class DepartmentUnitController : Controller
    {
        private readonly IService<DepartmentUnitEntity> _departmentUnitsService;
        private readonly IUserService _userService;
        private readonly IMapper _controllerMapper;
        private readonly ILogger<DepartmentUnitController> _logger;

        public DepartmentUnitController(IService<DepartmentUnitEntity> departmentUnitsService, IUserService userService, IMapper controllerMapper, ILogger<DepartmentUnitController> logger)
        {
            _departmentUnitsService = departmentUnitsService ?? throw new ArgumentNullException(nameof(departmentUnitsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DepartmentUnitViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/department-units/{id}
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
            var departmentUnitViewModel = _controllerMapper.Map<DepartmentUnitViewModel>(await _departmentUnitsService.GetAsync(id));

            return Ok(departmentUnitViewModel);
        }

        /// <summary>
        /// Gets the DepartmentUnitViewModel by user id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DepartmentUnitViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/users/{id}/department-units
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("~/api/users/{id}/department-units")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentUnitViewModel>> GetByUserId(Guid id)
        {
            var departmentUnitViewModel = _controllerMapper.Map<DepartmentUnitViewModel>(await _userService.GetAsync(id));

            return Ok(departmentUnitViewModel);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List of DepartmentUnitViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /department-units
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DepartmentUnitViewModel>>> GetAll()
        {
            var listOfDdepartmentUnitViewModel = (await _departmentUnitsService.GetAllAsync()).Select(depu => _controllerMapper.Map<DepartmentUnitViewModel>(depu));

            return Ok(listOfDdepartmentUnitViewModel);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /department-units/{id}
        ///
        /// </remarks>
        /// <response code="204">Model deleted</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentUnitsService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Posts the specified department unit view model.
        /// </summary>
        /// <param name="departmentUnitViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /department-units
        ///     {
        ///        "userId": 1,
        ///        "positionId": 1,
        ///        "DepartmentId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Model created</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(DepartmentUnitViewModel departmentUnitViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            var result = await _departmentUnitsService.AddAsync(_controllerMapper.Map<DepartmentUnitEntity>(departmentUnitViewModel));

            return Created($"department-units/{result}", result);
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="departmentUnitViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /department-units
        ///     {
        ///        "userId": 1,
        ///        "positionId": 1,
        ///        "DepartmentId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="404">Model not found</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, DepartmentUnitViewModel departmentUnitViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            var result = await _departmentUnitsService.UpdateAsync(id, _controllerMapper.Map<DepartmentUnitEntity>(departmentUnitViewModel));

            return NoContent();
        }
    }
}
