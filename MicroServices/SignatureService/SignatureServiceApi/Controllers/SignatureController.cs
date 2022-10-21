using Microsoft.AspNetCore.Mvc;
using SignatureService.Services.Interfaces;

namespace SignatureServiceApi.Controllers
{
    [ApiController]
    [Route("api/signatures")]
    public class SignatureController : Controller
    {
        private readonly ISignService _signService;

        public SignatureController(ISignService signService)
        {
            _signService = signService ?? throw new ArgumentNullException(nameof(signService));
        }

        /// <summary>
        /// Add signature for document.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="documentId">Document id</param>
        /// <param name="version">Version</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/signatures/{userId}/{documentId}/{version}
        ///
        /// </remarks>
        [HttpPost("{userId}/{documentId}/{version}")]
        public async Task<IActionResult> Post(Guid userId, Guid documentId, int version)
        {
            await _signService.AddAsync(userId, documentId, version);

            return Created($"/api/documents/{documentId}/{version}", new { DocumentId = documentId, Version = version });
        }

        /// <summary>
        /// Get users that signature document.
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <param name="version">Version</param>
        /// <returns>List of user id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/documents/{documentId}/{version}
        ///
        /// </remarks>
        [HttpGet("~/api/documents/{documentId}/{version}")]
        public async Task<IActionResult> Get(Guid documentId, int version)
        {
            var users = await _signService.GetUsersByDocumentIdAsync(documentId, version);

            return Ok(users);
        }

        /// <summary>
        /// Check user that signature document.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="documentId">Document id</param>
        /// <param name="version">Version</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/signatures/{userId}/{documentId}/{version}
        ///
        /// </remarks>
        [HttpGet("{userId}/{documentId}/{version}")]
        public async Task<IActionResult> Get(Guid userId, Guid documentId, int version)
        {
            var result = await _signService.CheckDocumentByUser(userId, documentId, version);

            return result ? Ok() : NotFound();
        }
    }
}
