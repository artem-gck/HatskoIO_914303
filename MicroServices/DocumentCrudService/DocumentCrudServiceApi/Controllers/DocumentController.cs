using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Commands.AddDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var query = new GetDocumentByIdQuery() { Id = id };

            var documentDto = (DocumentDto)(await _queryDispatcher.Send(query))[0];

            return File(documentDto.DocumentBody, "application/octet-stream", documentDto.FileName); ;
        }

        [HttpPost]
        public async Task<IActionResult> Post(DocumentViewModel documentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var file = GetByteArray(documentViewModel.File);

            var query = new AddDocumentCommand() 
            {
                DocumentName = documentViewModel.File.FileName,
                DocumentData = file
            };

            await _commandDispatcher.Send(query);

            return Created("document-names/{fileName}", query.DocumentName);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteDocumentCommand() { Id = id };

            await _commandDispatcher.Send(command);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put(DocumentViewModel documentViewModel)
        {
            var file = GetByteArray(documentViewModel.File);

            var query = new UpdateDocumentCommand()
            {
                DocumentName = documentViewModel.File.FileName,
                DocumentData = file
            };

            await _commandDispatcher.Send(query);

            return NoContent();
        }


        private byte[] GetByteArray(IFormFile file)
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
