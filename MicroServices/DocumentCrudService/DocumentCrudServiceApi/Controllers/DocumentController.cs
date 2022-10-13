using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Commands.AddDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById;
using DocumentCrudService.Cqrs.Realisation.Queries.IsDocumentExit;
using DocumentCrudService.ViewModels;
using DocumentCrudServiceApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        /// Gets the last version by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Last version of file</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/documents/{id}
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="404">File not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new IsDocumentExitQuery()
            {
                Id = id
            };
            var isExist = (DocumentExistDto)(await _queryDispatcher.Send(query))[0];

            return isExist.IsExist ? RedirectToActionPermanent("Get", new { id = id }) : NotFound();
        }

        /// <summary>
        /// Gets the last version by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Last version of file</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/documents/{id}/last-version
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="404">File not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("{id}/last-version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLastVersion(Guid id)
        {
            var query = new GetDocumentByIdQuery()
            {
                Id = id,
                Version = -1
            };
            var documentDto = (DocumentDto)(await _queryDispatcher.Send(query))[0];

            return File(documentDto.Body, "application/octet-stream", documentDto.Name);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <returns>File</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/documents/{id}/{version}
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="404">File not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("{id}/{version}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id, int version)
        {
            DocumentDto documentDto;

            var query = new GetDocumentByIdQuery() 
            { 
                Id = id,
                Version = version
            };
            documentDto = (DocumentDto)(await _queryDispatcher.Send(query))[0];

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
        ///        "createrId": "81f77f19-24f2-43a2-a35f-41975c1d785a"
        ///        "file": byte[],
        ///     }
        ///
        /// </remarks>
        /// <response code="201">File created</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
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
                CreaterId = documentViewModel.CreaterId,
                Name = documentViewModel.File.FileName,
                Body = file
            };
            var id = (IdDto)(await _commandDispatcher.Send(command));

            return Created($"/api/documents/{id.Id}", id.Id);
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
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
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
        ///        "id": "81f77f19-24f2-43a2-a35f-41975c1d785a",
        ///        "createrId": "1b2edca4-29a2-4a00-adde-3b6041cb60d1",
        ///        "file": byte[],
        ///     }
        ///
        /// </remarks>
        /// <response code="204">File updated</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(UpdateDocumentRequest documentViewModel)
        {
            var file = GetByteArray(documentViewModel.File);

            var command = new UpdateDocumentCommand()
            {
                Id = documentViewModel.Id,
                CreaterId = documentViewModel.CreaterId,
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
