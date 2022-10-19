namespace NotificationService.DataAccess.DataBase.Exceptions
{
    public class NotFoundMessageException : Exception
    {
        public NotFoundMessageException(Guid id)
            : base($"Not found message with id = {id}")
        {

        }
    }
}
