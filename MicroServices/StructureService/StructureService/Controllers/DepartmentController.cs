using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services.Dto;
using StructureService.Application.Services;
using StructureService.ViewModels;

namespace StructureService.Controllers
{
    [Route("departments")]
    public class DepartmentController : Controller
    {
        private readonly IService<DepartmentDto> _departmentsService;
        private readonly IMapper _controllerMapper;

        public DepartmentController(IService<DepartmentDto> departmentsService, IMapper controllerMapper)
        {
            _departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentViewModel>> Get(int id)
        {
            var departmentViewModel = _controllerMapper.Map<DepartmentViewModel>(await _departmentsService.GetAsync(id));

            return Ok(departmentViewModel);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> GetAll()
        {
            var listOfDepartmentViewModel = (await _departmentsService.GetAllAsync()).Select(dep => _controllerMapper.Map<DepartmentViewModel>(dep));

            return Ok(listOfDepartmentViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentsService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(DepartmentViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentsService.AddAsync(_controllerMapper.Map<DepartmentDto>(departmentViewModel));

            return Created($"departments/{result}", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, DepartmentViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentsService.UpdateAsync(id, _controllerMapper.Map<DepartmentDto>(departmentViewModel));

            return NoContent();
        }
    }
}
