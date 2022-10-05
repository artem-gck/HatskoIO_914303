using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Queries
{
    public interface IQueryHandler<T> : IQueryHandler where T : IQuery
    {
        Task<IList<IResult>> Handle(T query);
    }
}
