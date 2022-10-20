using Microsoft.AspNetCore.Mvc;
using SignatureService.Services.Interfaces;
using System.Diagnostics.Contracts;

namespace SignatureServiceApi.Controllers
{
    [ApiController]
    [Route("api/signatures")]
    public class SignatureController : Controller
    {
        private readonly ISignatureService _signatureService;

        public SignatureController(ISignatureService signatureService)
        {
            _signatureService = signatureService ?? throw new ArgumentNullException(nameof(signatureService));
        }

        [HttpPost("{userId}/{documentId}/{version}")]
        public async Task<IActionResult> Post(Guid userId, Guid documentId, int version)
        {
            await _signatureService.AddAsync(userId, documentId, version);

            return Created("", "");
        }

        [HttpGet("~/api/documents/{documentId}/{version}")]
        public async Task<IActionResult> Get(Guid documentId, int version)
        {
            var users = await _signatureService.GetUsersByDocumentIdAsync(documentId, version);

            return Ok(users);
        }

        [HttpGet("{userId}/{documentId}/{version}")]
        public async Task<IActionResult> Get(Guid userId, Guid documentId, int version)
        {
            var result = await _signatureService.CheckDocumentByUser(userId, documentId, version);

            return result ? Ok() : NotFound();
        }
    }
}
