using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureService.Application.Services.Dto;
using StructureService.ViewModels;

namespace StructureService.Controllers
{
    [Route("positions")]
    public class PositionsController : Controller
    {
        private readonly IService<PositionDto> _positionsService;
        private readonly IMapper _controllerMapper;

        public PositionsController(IService<PositionDto> positionsService, IMapper controllerMapper)
        {
            _positionsService = positionsService ?? throw new ArgumentNullException(nameof(positionsService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PositionViewModel>> Get(int id)
        {
            var positionViewModel = _controllerMapper.Map<PositionViewModel>(await _positionsService.GetAsync(id));

            return Ok(positionViewModel);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PositionViewModel>>> GetAll()
        {
            var listOfPositonViewModel = (await _positionsService.GetAllAsync()).Select(pos => _controllerMapper.Map<PositionViewModel>(pos));

            return Ok(listOfPositonViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _positionsService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(PositionViewModel positionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _positionsService.AddAsync(_controllerMapper.Map<PositionDto>(positionViewModel));

            return Created($"positions/{result}", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, PositionViewModel positionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _positionsService.UpdateAsync(id, _controllerMapper.Map<PositionDto>(positionViewModel));

            return NoContent();
        }
    }
}
