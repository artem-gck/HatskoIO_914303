using DocumentCrudService.Cqrs.Exceptions;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _service;

        public QueryDispatcher(IServiceProvider service)
        {
            _service = service;
        }

        public async Task<IList<IResult>> Send<T>(T query) where T : IQuery
        {
            var handler = _service.GetService(typeof(IQueryHandler<T>));

            if (handler != null)
                return await ((IQueryHandler<T>)handler).Handle(query);
            else
                throw new NotFoundServiceException($"Query doesn't have any handler {query.GetType().Name}");
        }
    }
}
