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
    [ApiVersion("2.0")]
    [Authorize]
    public class TaskControllerV2 : Controller
    {
        private readonly IService<TaskEntity> _taskService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        private readonly IValidator<CreateTaskRequest> _createValidator;
        private readonly IValidator<UpdateTaskRequest> _updateValidator;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IConfiguration _configuration;

        public TaskControllerV2(IService<TaskEntity> taskService, IMapper mapper, ILogger<TaskService> logger, IValidator<CreateTaskRequest> createValidator, IValidator<UpdateTaskRequest> updateValidator, ISendEndpointProvider sendEndpointProvider, IConfiguration config)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
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
        ///       "type": "string",
        ///       "header": "string",
        ///       "ownerUserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "deadLine": "2022-10-02T20:07:01.294Z",
        ///       "performers": [
        ///          {
        ///             "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "typeOfTask": "signature",
        ///             "description": "qwe"
        ///          }
        ///        ],
        ///        "documents": [
        ///          {
        ///             "documentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
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
            var validationResult = await _createValidator.ValidateAsync(taskViewModel);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                _logger.LogDebug("Invalid model state {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var result = await _taskService.AddAsync(_mapper.Map<TaskEntity>(taskViewModel));

            var uri = _configuration.GetConnectionString("ServiceBus").Split(";")[0][9..] + _configuration["Queues:NewTask"];

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(uri));

            await sendEndpoint.Send(new NewTaskMessage
            {
                Id = result,
                OwnerUserId = taskViewModel.OwnerUserId,
                DeadLine = taskViewModel.DeadLine,
                Header = taskViewModel.Header,
            });

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
            var validationResult = await _updateValidator.ValidateAsync(taskViewModel);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                _logger.LogDebug("Invalid model state {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            await _taskService.UpdateAsync(id, _mapper.Map<TaskEntity>(taskViewModel));

            var uri = _configuration.GetConnectionString("ServiceBus").Split(";")[0][9..] + _configuration["Queues:UpdateTask"];

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(uri));

            await sendEndpoint.Send(new UpdateTaskMessage
            {
                Id = id,
                OwnerUserId = taskViewModel.OwnerUserId,
                DeadLine = taskViewModel.DeadLine,
                Header = taskViewModel.Header,
            });

            return NoContent();
        }
    }
}
