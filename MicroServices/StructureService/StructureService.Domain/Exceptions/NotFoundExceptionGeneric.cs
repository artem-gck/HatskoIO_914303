using StructureService.Domain.Entities;

namespace StructureService.Domain.Exceptions
{
    public class NotFoundException<T> : NotFoundException where T : BaseEntity
    {
        public NotFoundException(Guid id)
            : base($"{typeof(T).Name} with id = {id} not found")
        {

        }

        public NotFoundException(string name)
            : base($"{typeof(T).Name} with name = {name} not found")
        {

        }
    }
}
