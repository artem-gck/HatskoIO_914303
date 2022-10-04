using DocumentCrudService.Application.Services.Commands;
using DocumentCrudService.Application.Services.Dto;
using DocumentCrudService.Application.Services.Queries;
using DocumentCrudService.Infrastructure.Realisation.Commands.AddDocument;
using DocumentCrudService.Infrastructure.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Infrastructure.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentById;
using DocumentCrudService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DocumentCrudService.Controllers
{
    [Route("documents")]
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

            var listOfFile = await _queryDispatcher.Send(query);
            var documentDto = (DocumentDto)listOfFile[0];

            var file = GetFormFile(documentDto);

            var documentViewModel = new DocumentViewModel()
            {
                File = file
            };

            return Ok(documentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post(DocumentViewModel documentViewModel)
        {
            var file = GetByteArray(documentViewModel.File);

            var query = new AddDocumentCommand() 
            {
                DocumentName = documentViewModel.File.FileName,
                DocumentData = file
            };

            await _commandDispatcher.Send(query);

            return Created("qwe", "qwe");
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

        private IFormFile GetFormFile(DocumentDto document)
        {
            var stream = new MemoryStream(document.DocumentBody);

            return new FormFile(stream, 0, document.DocumentBody.Length, document.FileName, document.FileName);
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
