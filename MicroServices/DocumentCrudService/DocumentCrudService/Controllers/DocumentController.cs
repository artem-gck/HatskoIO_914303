using DocumentCrudService.Application.Services.Commands;
using DocumentCrudService.Application.Services.Queries;
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
