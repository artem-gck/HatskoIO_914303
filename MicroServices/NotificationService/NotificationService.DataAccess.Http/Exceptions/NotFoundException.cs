namespace NotificationService.DataAccess.Http.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid id)
            : base($"Not found entity with id = {id}")
        {

        }
    }
}
