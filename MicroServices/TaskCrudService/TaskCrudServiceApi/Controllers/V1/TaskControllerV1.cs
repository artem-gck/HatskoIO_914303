using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MassTransit.Transports;
using Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskCrudService.Adapters.Output;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Ports.Output;
using TaskCrudServiceApi.ViewModels.CreateRequest;
using TaskCrudServiceApi.ViewModels.Responce;
using TaskCrudServiceApi.ViewModels.UpdateRequest;

namespace TaskCrudServiceApi.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tasks")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    //[Authorize]
    public class TaskControllerV1 : Controller
    {
        private readonly IService<TaskEntity> _taskService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IConfiguration _configuration;

        public TaskControllerV1(IService<TaskEntity> taskService, IMapper mapper, ILogger<TaskService> logger, ISendEndpointProvider sendEndpointProvider, IConfiguration config)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List of TaskViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /tasks
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="500">Internal server error</response>
        [ApiVersionNeutral]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskResponce>>> Get()
        {
            var listOfTaskViewModel = _mapper.Map<IEnumerable<TaskResponce>>(await _taskService.GetAsync());

            return Ok(listOfTaskViewModel);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /tasks/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [ApiVersionNeutral]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            var taskViewModel = _mapper.Map<TaskResponce>(await _taskService.GetAsync(id));

            return Ok(taskViewModel);
        }

        /// <summary>
        /// Gets by the user id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /tasks/users/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [ApiVersionNeutral]
        [HttpGet("~/api/v{version:apiVersion}/users/{id}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByUserId(Guid id, string? status)
        {
            var listTaskViewModel = _mapper.Map<IEnumerable<TaskResponce>>(await _taskService.GetByNameAync(id, status));

            return Ok(listTaskViewModel);
        }

        /// <summary>
        /// Gets by the user id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /tasks/users/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [ApiVersionNeutral]
        [HttpGet("~/api/v{version:apiVersion}/performer/{id}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByPerformerId(Guid id, string? status)
        {
            var listTaskViewModel = _mapper.Map<IEnumerable<TaskResponce>>(await _taskService.GetByPerformerId(id, status));

            return Ok(listTaskViewModel);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /tasks/{id}
        ///
        /// </remarks>
        /// <response code="204">Model deleted</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [ApiVersionNeutral]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskService.DeleteAsync(id);

            var uri = _configuration.GetConnectionString("ServiceBus").Split(";")[0][9..] + _configuration["Queues:DeleteTask"];

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(uri));

            await sendEndpoint.Send(new DeleteTaskMessage
            {
                Id = id,
            });

            return NoContent();
        }

        /// <summary>
        /// Posts the specified department view model.
        /// </summary>
        /// <param name="taskViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /tasks
        ///     {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "type": "string",
        ///       "header": "string",
        ///       "ownerUserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "deadLine": "2022-10-02T20:07:01.294Z",
        ///       "arguments": [
        ///         {
        ///            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///            "argumentType": "string",
        ///            "value": "string"
        ///          }
        ///        ]
        ///      }
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
        public async Task<IActionResult> Post([FromBody] CreateTaskRequest taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Invalid model state {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var result = await _taskService.AddAsync(_mapper.Map<TaskEntity>(taskViewModel));

            return Created($"tasks/{result}", result);
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="taskViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     POST /tasks/{id}
        ///     {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "type": "string",
        ///       "header": "string",
        ///       "ownerUserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "deadLine": "2022-10-02T20:07:01.294Z",
        ///       "arguments": [
        ///         {
        ///            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///            "argumentType": "string",
        ///            "value": "string"
        ///          }
        ///        ]
        ///      }
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
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateTaskRequest taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Invalid model state {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            await _taskService.UpdateAsync(id, _mapper.Map<TaskEntity>(taskViewModel));

            return NoContent();
        }
    }
}
