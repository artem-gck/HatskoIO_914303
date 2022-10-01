using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services.Dto;
using StructureService.Application.Services;
using StructureService.ViewModels;

namespace StructureService.Controllers
{
    [Route("department-units")]
    public class DepartmentUnitController : Controller
    {
        private readonly IService<DepartmentUnitDto> _departmentUnitsService;
        private readonly IMapper _controllerMapper;

        public DepartmentUnitController(IService<DepartmentUnitDto> departmentUnitsService, IMapper controllerMapper)
        {
            _departmentUnitsService = departmentUnitsService ?? throw new ArgumentNullException(nameof(departmentUnitsService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentUnitViewModel>> Get(int id)
        {
            var departmentUnitViewModel = _controllerMapper.Map<DepartmentUnitViewModel>(await _departmentUnitsService.GetAsync(id));

            return Ok(departmentUnitViewModel);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DepartmentUnitViewModel>>> GetAll()
        {
            var listOfDdepartmentUnitViewModel = (await _departmentUnitsService.GetAllAsync()).Select(depu => _controllerMapper.Map<DepartmentUnitViewModel>(depu));

            return Ok(listOfDdepartmentUnitViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentUnitsService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(DepartmentUnitViewModel departmentUnitViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentUnitsService.AddAsync(_controllerMapper.Map<DepartmentUnitDto>(departmentUnitViewModel));

            return Created($"department-units/{result}", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, DepartmentUnitViewModel departmentUnitViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentUnitsService.UpdateAsync(id, _controllerMapper.Map<DepartmentUnitDto>(departmentUnitViewModel));

            return NoContent();
        }
    }
}
