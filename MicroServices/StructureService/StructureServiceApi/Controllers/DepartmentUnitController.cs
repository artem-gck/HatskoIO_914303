﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureService.Domain.Entities;
using StructureService.Infrastructure.Services;
using StructureServiceApi.ViewModels.AddRequest;
using StructureServiceApi.ViewModels.Responce;
using StructureServiceApi.ViewModels.UpdateRequest;

namespace StructureServiceApi.Controllers
{
    [Route("api/department-units")]
    [Produces("application/json")]
    public class DepartmentUnitController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _controllerMapper;
        private readonly ILogger<DepartmentUnitController> _logger;

        public DepartmentUnitController(IUserService userService, IMapper controllerMapper, ILogger<DepartmentUnitController> logger)
        {
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
        public async Task<ActionResult<DepartmentUnitResponce>> Get(Guid id)
        {
            var departmentUnitViewModel = _controllerMapper.Map<DepartmentUnitResponce>(await _userService.GetAsync(id));

            return Ok(departmentUnitViewModel);
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
            await _userService.DeleteAsync(id);

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
        public async Task<IActionResult> Post(AddDepartmentUnitRequest departmentUnitViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            var result = await _userService.AddAsync(_controllerMapper.Map<User>(departmentUnitViewModel));

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
        public async Task<IActionResult> Put(Guid id, UpdateDepartmentUnitRequest departmentUnitViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            await _userService.UpdateAsync(id, _controllerMapper.Map<User>(departmentUnitViewModel));

            return NoContent();
        }
    }
}
