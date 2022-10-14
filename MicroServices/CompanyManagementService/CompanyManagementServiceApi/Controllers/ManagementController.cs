using AutoMapper;
using CompanyManagementService.Services.Interfaces;
using CompanyManagementServiceApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagementServiceApi.Controllers
{
    [ApiController]
    [Route("management")]
    public class ManagementController : Controller
    {
        private readonly IStructureService _structureService;
        private readonly IMapper _mapper;

        public ManagementController(IStructureService structureService, IMapper mapper)
        {
            _structureService = structureService ?? throw new ArgumentNullException(nameof(structureService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cheifStructureResponce = _mapper.Map<CheifStructureResponce>(await _structureService.GetCheifStructure(id));

            return Ok(cheifStructureResponce);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userResponce = _mapper.Map<UserResponce>(await _structureService.GetUser(id));

            return Ok(userResponce);
        }
    }
}
