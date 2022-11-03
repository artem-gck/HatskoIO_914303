using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureService.Domain.Entities;
using StructureServiceApi.ViewModels.AddRequest;
using StructureServiceApi.ViewModels.Responce;
using StructureServiceApi.ViewModels.UpdateRequest;

namespace StructureServiceApi.Controllers
{
    [Produces("application/json")]
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IService<DepartmentEntity> _departmentsService;
        private readonly IMapper _controllerMapper;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IService<DepartmentEntity> departmentsService, IMapper controllerMapper, ILogger<DepartmentController> logger)
        {
            _departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DepartmentViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /departments/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("api/departments/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentResponce>> Get(Guid id)
        {
            var departmentViewModel = _controllerMapper.Map<DepartmentResponce>(await _departmentsService.GetAsync(id));

            return Ok(departmentViewModel);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List of DepartmentViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /departments
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("api/departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DepartmentResponce>>> Get()
        {
            var listOfDepartmentViewModel = (await _departmentsService.GetAllAsync()).Select(dep => _controllerMapper.Map<DepartmentResponce>(dep));

            return Ok(listOfDepartmentViewModel);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /departments/{id}
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("api/departments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmentsService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Posts the specified department view model.
        /// </summary>
        /// <param name="departmentViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /departments
        ///     {
        ///        "name": "qwe",
        ///        "cheifUserId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Model created</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("api/departments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] AddDepartmentRequest departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            var result = await _departmentsService.AddAsync(_controllerMapper.Map<DepartmentEntity>(departmentViewModel));

            return Created($"departments/{result}", result);
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="departmentViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /departments
        ///     {
        ///        "name": "qwe",
        ///        "cheifUserId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="404">Model not found</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        /// <returns></returns>
        [HttpPut("api/departments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateDepartmentRequest departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            await _departmentsService.UpdateAsync(id, _controllerMapper.Map<DepartmentEntity>(departmentViewModel));

            return NoContent();
        }
    }
}
