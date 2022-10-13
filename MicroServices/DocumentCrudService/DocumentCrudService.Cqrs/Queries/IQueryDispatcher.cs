using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Queries
{
    public interface IQueryDispatcher
    {
        public Task<IList<IResult>> Send<T>(T query) where T : IQuery;
    }
}
