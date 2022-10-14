using CompanyManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagementServiceApi.Controllers
{
    [ApiController]
    [Route("qwe")]
    public class HomeController : Controller
    {
        private readonly IStructureService _structureService;

        public HomeController(IStructureService structureService)
        {
            _structureService = structureService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var a = await _structureService.GetCheifStructure(id);

            return Ok(a);
        }
    }
}
