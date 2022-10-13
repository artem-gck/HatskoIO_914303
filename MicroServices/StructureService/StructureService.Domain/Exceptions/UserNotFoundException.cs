namespace StructureService.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid id)
            : base($"User with id = {id} not found")
        {

        }
    }
}
