using DocumentCrudService.Application.Services.Results;
using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Application.Services.Dispatchers
{
    public interface IQueryDispatcher
    {
        public Task<IList<IResult>> Send<T>(T query) where T : IQuery;
    }
}
