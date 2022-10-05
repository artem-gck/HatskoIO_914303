using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Commands.AddDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName;
using DocumentCrudService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DocumentCrudService.Controllers
{
    [Route("documents")]
    [Produces("application/json")]
    public class DocumentController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public DocumentController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>File</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET documents/{id}
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="404">File not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string id)
        {
            var query = new GetDocumentByIdQuery() { Id = id };
            var documentDto = (DocumentDto)(await _queryDispatcher.Send(query))[0];

            return File(documentDto.Body, "application/octet-stream", documentDto.Name);
        }

        /// <summary>
        /// Gets file by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <returns>File</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET documents/name/{name}/version/{version}
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="404">File not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("name/{name}/version/{version}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByName(string name, int version = -1)
        {
            var query = new GetDocumentByNameQuery()
            {
                Name = name,
                Version = version
            };
            var documentDto = (DocumentDto)(await _queryDispatcher.Send(query))[0];

            return File(documentDto.Body, "application/octet-stream", documentDto.Name);
        }

        /// <summary>
        /// Posts the specified document view model.
        /// </summary>
        /// <param name="documentViewModel">The document view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /documents
        ///     {
        ///        "file": byte[],
        ///     }
        ///
        /// </remarks>
        /// <response code="201">File created</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateDocumentRequest documentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var file = GetByteArray(documentViewModel.File);

            var query = new AddDocumentCommand() 
            {
                Name = documentViewModel.File.FileName,
                Body = file
            };
            await _commandDispatcher.Send(query);

            return Created("document-names/{fileName}", query.Name);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE documents/{id}
        ///
        /// </remarks>
        /// <response code="204">File deleted</response>
        /// <response code="404">File not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteDocumentCommand() { Id = id };
            await _commandDispatcher.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Puts the specified document view model.
        /// </summary>
        /// <param name="documentViewModel">The document view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /documents
        ///     {
        ///        "file": byte[],
        ///     }
        ///
        /// </remarks>
        /// <response code="204">File updated</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(CreateDocumentRequest documentViewModel)
        {
            var file = GetByteArray(documentViewModel.File);

            var query = new UpdateDocumentCommand()
            {
                Name = documentViewModel.File.FileName,
                Body = file
            };
            await _commandDispatcher.Send(query);

            return NoContent();
        }


        private static byte[] GetByteArray(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var ms = new MemoryStream();

                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                return fileBytes;
            }

            return null;
        }
    }
}
