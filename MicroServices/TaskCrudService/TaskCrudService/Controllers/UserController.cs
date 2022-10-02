using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskCrudService.Application.Services;
using TaskCrudService.ViewModels;

namespace TaskCrudService.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetAll(Guid id)
        {
            var listOfTaskViewModel = _mapper.Map<IEnumerable<TaskViewModel>>(await _userService.GetAllAsync(id));

            return Ok(listOfTaskViewModel);
        }
    }
}
