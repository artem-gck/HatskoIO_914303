namespace SignatureService.DataAccess.Http.Exceptions
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(Guid id)
            : base($"Document with id = {id} not found")
        {

        }
    }
}
