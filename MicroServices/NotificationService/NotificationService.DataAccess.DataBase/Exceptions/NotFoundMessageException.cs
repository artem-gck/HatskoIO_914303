namespace NotificationService.DataAccess.Http.Exceptions
{
    public class NotFoundMessageException : Exception
    {
        public NotFoundMessageException(Guid id)
            : base($"Not found message with id = {id}")
        {

        }
    }
}
