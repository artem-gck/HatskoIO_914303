using Microsoft.AspNetCore.Mvc;
using SignatureService.Services.Interfaces;
using System.Diagnostics.Contracts;

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

        [HttpPost("{userId}/{documentId}/{version}")]
        public async Task<IActionResult> Post(Guid userId, Guid documentId, int version)
        {
            await _signService.AddAsync(userId, documentId, version);

            return Created("", "");
        }

        [HttpGet("~/api/documents/{documentId}/{version}")]
        public async Task<IActionResult> Get(Guid documentId, int version)
        {
            var users = await _signService.GetUsersByDocumentIdAsync(documentId, version);

            return Ok(users);
        }

        [HttpGet("{userId}/{documentId}/{version}")]
        public async Task<IActionResult> Get(Guid userId, Guid documentId, int version)
        {
            var result = await _signService.CheckDocumentByUser(userId, documentId, version);

            return result ? Ok() : NotFound();
        }
    }
}
