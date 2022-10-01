using StructureService.Domain.Entities;

namespace StructureService.Domain.Exceptions
{
    public class NotFoundException<T> : NotFoundException where T : BaseEntity
    {
        public NotFoundException(int id)
            : base($"{nameof(T)} with id = {id} not found")
        {

        }
    }
}
