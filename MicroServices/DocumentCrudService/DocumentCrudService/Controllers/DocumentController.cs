using DocumentCrudService.Application.Services.Dispatchers;
using DocumentCrudService.Infrastructure.Realisation.Commands.AddDocument;
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
    }
}
