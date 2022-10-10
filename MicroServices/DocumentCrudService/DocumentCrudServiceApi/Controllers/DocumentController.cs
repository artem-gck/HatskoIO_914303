using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Commands.AddDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName;
using DocumentCrudService.ViewModels;
using DocumentCrudServiceApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DocumentCrudService.Controllers
{
    [Route("api/documents")]
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
        /// <param name="name">The name of document.</param>
        /// <param name="version">The version.</param>
        /// <returns>File</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/documents
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="404">File not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string? id, string? name, int version = -1)
        {
            DocumentDto documentDto;

            if (id is not null)
            {
                var query = new GetDocumentByIdQuery() { Id = id };
                documentDto = (DocumentDto)(await _queryDispatcher.Send(query))[0];
            }
            else if (name is not null)
            {
                var query = new GetDocumentByNameQuery()
                {
                    Name = name,
                    Version = version
                };
                documentDto = (DocumentDto)(await _queryDispatcher.Send(query))[0];
            }
            else
                return BadRequest();

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
        ///     POST /api/documents
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

            var command = new AddDocumentCommand() 
            {
                Name = documentViewModel.File.FileName,
                Body = file
            };
            var result = await _commandDispatcher.Send(command);
            var id = ((IdDto)result).Id;

            return Created("api/documents/{id}", id);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/documents/{id}
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
        ///     PUT /api/documents
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

            var command = new UpdateDocumentCommand()
            {
                Name = documentViewModel.File.FileName,
                Body = file
            };
            await _commandDispatcher.Send(command);

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
