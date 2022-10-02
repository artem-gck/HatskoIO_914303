using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskCrudService.Application.Services;
using TaskCrudService.Application.Services.Dto;
using TaskCrudService.ViewModels;

namespace TaskCrudService.Controllers
{
    [Route("tasks")]
    public class TaskController : Controller
    {
        private readonly IService<TaskDto> _taskService;
        private readonly IMapper _mapper;

        public TaskController(IService<TaskDto> taskService, IMapper mapper)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetAll()
        {
            var listOfTaskViewModel = _mapper.Map<IEnumerable<TaskViewModel>>(await _taskService.GetAllAsync());

            return Ok(listOfTaskViewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> Get(Guid id)
        {
            var taskViewModel = _mapper.Map<TaskViewModel>(await _taskService.GetAsync(id));

            return Ok(taskViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _taskService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _taskService.AddAsync(_mapper.Map<TaskDto>(taskViewModel));

            return Created($"tasks/{result}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] TaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _taskService.UpdateAsync(id, _mapper.Map<TaskDto>(taskViewModel));

            return NoContent();
        }
    }
}
