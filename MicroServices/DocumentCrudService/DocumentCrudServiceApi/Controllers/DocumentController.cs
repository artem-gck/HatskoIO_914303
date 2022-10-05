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
using IResult = DocumentCrudService.Cqrs.Results.IResult;

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

        [HttpGet("{filter}/{id}/{version}")]
        public async Task<IActionResult> Get(string filter, string id, int version = -1)
        {
            IResult result;

            if (filter == "id")
            {
                var query = new GetDocumentByIdQuery() { Id = id };

                result = (await _queryDispatcher.Send(query))[0];
            }
            else if (filter == "name")
            {
                var query = new GetDocumentByNameQuery()
                {
                    Name = id,
                    Version = version
                };

                result = (await _queryDispatcher.Send(query))[0];
            }
            else
                return BadRequest();

            var documentDto = (DocumentDto)result;

            return File(documentDto.Body, "application/octet-stream", documentDto.Name);
        }

        [HttpPost]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteDocumentCommand() { Id = id };

            await _commandDispatcher.Send(command);

            return NoContent();
        }

        [HttpPut]
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
