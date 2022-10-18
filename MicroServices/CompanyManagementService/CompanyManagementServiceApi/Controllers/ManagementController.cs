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
        /// <response code="200">Return cheif structure</response>
        /// <response code="400">Model state is invalid</response>
        /// <response code="404">Cheif not found</response>
        /// <response code="409">Database problems</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("cheifs/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            var cheifStructureResponce = _mapper.Map<CheifStructureResponce>(await _structureService.GetCheifStructureAsync(id));

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
        /// <response code="200">Return user</response>
        /// <response code="400">Model state is invalid</response>
        /// <response code="404">User not found</response>
        /// <response code="409">Database problems</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("users/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userResponce = _mapper.Map<UserResponce>(await _structureService.GetUserAsync(id));

            return Ok(userResponce);
        }
    }
}
