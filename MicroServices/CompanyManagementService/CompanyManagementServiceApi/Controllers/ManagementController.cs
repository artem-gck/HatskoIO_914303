using CompanyManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagementServiceApi.Controllers
{
    [ApiController]
    [Route("management")]
    public class ManagementController : Controller
    {
        private readonly IStructureService _structureService;

        public ManagementController(IStructureService structureService)
        {
            _structureService = structureService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _structureService.GetCheifStructure(id));
        }
    }
}
