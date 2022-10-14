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

        /// <summary>
        /// Get cheif structure by cheif id
        /// </summary>
        /// <param name="id">Cheif id</param>
        /// <returns>Cheif structure</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/management/cheifs/{id}
        ///
        /// </remarks>
        [HttpGet("cheifs/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cheifStructureResponce = _mapper.Map<CheifStructureResponce>(await _structureService.GetCheifStructure(id));

            return Ok(cheifStructureResponce);
        }

        /// <summary>
        /// Get full user info
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User info</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/management/users/{id}
        ///
        /// </remarks>
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userResponce = _mapper.Map<UserResponce>(await _structureService.GetUser(id));

            return Ok(userResponce);
        }
    }
}
