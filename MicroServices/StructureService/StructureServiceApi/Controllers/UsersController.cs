using AutoMapper;
using MassTransit;
using MassTransit.Transports;
using Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureService.Domain.Entities;
using StructureService.Infrastructure.Services;
using StructureServiceApi.ViewModels.AddRequest;
using StructureServiceApi.ViewModels.Responce;
using StructureServiceApi.ViewModels.UpdateRequest;
using System.Runtime.CompilerServices;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace StructureServiceApi.Controllers
{
    [Produces("application/json")]
    //[Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IService<PositionEntity> _positionService;
        private readonly IMapper _controllerMapper;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<UsersController> _logger;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, IService<PositionEntity> positionService, IMapper controllerMapper, ISendEndpointProvider sendEndpointProvider, ILogger<UsersController> logger, IConfiguration config)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _positionService = positionService ?? throw new ArgumentNullException(nameof(positionService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="departmentId">The identifier of department.</param>
        /// <param name="userId">The identifier of department.</param>
        /// <returns>DepartmentUnitViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/departments/{departmentId}/users/{userId}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("api/departments/{departmentId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponce>> Get(Guid departmentId, Guid userId)
        {
            var userViewModel = _controllerMapper.Map<UserResponce>(await _userService.GetAsync(departmentId, userId));

            return Ok(userViewModel);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="departmentId">The identifier of department.</param>
        /// <returns>DepartmentUnitViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/departments/{departmentId}/users
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("api/departments/{departmentId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponce>> Get(Guid departmentId)
        {
            var listOfUserViewModel = _controllerMapper.Map<IEnumerable<UserResponce>>(await _userService.GetByDepartmentId(departmentId));

            return Ok(listOfUserViewModel);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="departmentId">The identifier of department.</param>
        /// <param name="userId">The identifier of department.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/departments/{departmentId}/users/{userId}
        ///
        /// </remarks>
        /// <response code="204">Model deleted</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        //[HttpDelete("api/departments/{departmentId}/users/{userId}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Delete(Guid departmentId, Guid userId)
        //{
        //    await _userService.DeleteAsync(departmentId, userId);

        //    return NoContent();
        //}

        /// <summary>
        /// Posts the specified department unit view model.
        /// </summary>
        /// <param name="departmentId">The identifier of department.</param>
        /// <param name="userViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/departments/{departmentId}/users
        ///     {
        ///        "id": "a8a472d3-b7da-49f7-abae-28df32cc05b0",
        ///        "salary": 123
        ///        "position": "qwe",
        ///        "cheifUserId": "a8a472d3-b7da-49f7-abae-28df32cc05b0"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Model created</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        //[HttpPost("api/departments/{departmentId}/users")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Post(Guid departmentId, AddUserRequest userViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

        //        return BadRequest(ModelState);
        //    }

        //    var result = await _userService.AddAsync(departmentId, _controllerMapper.Map<UserEntity>(userViewModel));

        //    return Created($"api/departments/{departmentId}/users/{result}", result);
        //}

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="departmentId">The identifier of department.</param>
        /// <param name="userId">The identifier of department.</param>
        /// <param name="userViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/departments/{departmentId}/users/{userId}
        ///     {
        ///        "id": "a8a472d3-b7da-49f7-abae-28df32cc05b0",
        ///        "salary": 123
        ///        "position": "qwe",
        ///        "cheifUserId": "a8a472d3-b7da-49f7-abae-28df32cc05b0"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="404">Model not found</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("api/departments/{departmentId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid departmentId, Guid userId, [FromBody] UpdateUserRequest userViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            await _userService.UpdateAsync(departmentId, userId, _controllerMapper.Map<UserEntity>(userViewModel));

            var uri = _configuration.GetConnectionString("ServiceBus").Split(";")[0][9..] + _configuration["Queues:UpdateUser"];

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(uri));

            await sendEndpoint.Send(new UpdateUserMessage
            {
                Id = userViewModel.Id,
                DepartmentId = departmentId,
                PositionId = userViewModel.PositionId,
            });

            return NoContent();
        }
    }
}
