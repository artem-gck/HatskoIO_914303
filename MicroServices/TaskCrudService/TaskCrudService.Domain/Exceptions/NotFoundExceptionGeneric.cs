using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Domain.Exceptions
{
    public class NotFoundException<T> : Exception where T : BaseEntity
    {
        public NotFoundException(Guid id)
            : base($"Not found {typeof(T).Name} by id = {id}")
        {

        }
    }
}
