using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Ports.Output;

namespace TaskCrudService.Controllers
{
    [Route("tasks")]
    [Produces("application/json")]
    public class TaskController : Controller
    {
        private readonly IService<TaskEntity> _taskService;

        public TaskController(IService<TaskEntity> taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetAll()
        {
            var listOfTaskViewModel = await _taskService.GetAllAsync();

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
        [HttpGet("{filter}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskEntity>> Get(string filter, Guid id)
        {
            if (filter == "user")
            {
                var taskViewModel = await _taskService.GetByNameAync(id);

                return Ok(taskViewModel);
            }
            else if (filter == "task")
            {
                var taskViewModel = await _taskService.GetAsync(id);

                return Ok(taskViewModel);
            }
            else
                return BadRequest();

            
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
        /// <response code="204">Model saved</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskService.DeleteAsync(id);

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
        public async Task<IActionResult> Post([FromBody] TaskEntity taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _taskService.AddAsync(taskViewModel);

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
        public async Task<IActionResult> Put(Guid id, [FromBody] TaskEntity taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskService.UpdateAsync(id, taskViewModel);

            return NoContent();
        }
    }
}
