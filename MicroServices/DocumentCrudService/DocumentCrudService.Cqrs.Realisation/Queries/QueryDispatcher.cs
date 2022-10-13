using DocumentCrudService.Cqrs.Exceptions;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Cqrs.Units;
using Microsoft.Extensions.Logging;

namespace DocumentCrudService.Cqrs.Realisation.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _service;
        private readonly ILogger<QueryDispatcher> _logger;

        public QueryDispatcher(IServiceProvider service, ILogger<QueryDispatcher> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<IResult>> Send<T>(T query) where T : IQuery
        {
            var handler = _service.GetService(typeof(IQueryHandler<T>));

            if (handler is IQueryHandler<T> queryHandler)
                return await queryHandler.Handle(query);
            else
            {
                var exception = new NotFoundServiceException($"Query doesn't have any handler {query.GetType().Name}");

                _logger.LogWarning(exception, "Not found service with name {Name}", query.GetType().Name);
                throw exception;
            }
        }
    }
}
