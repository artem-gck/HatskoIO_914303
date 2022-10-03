namespace DocumentCrudService.Domain.Exceptions
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string name)
            : base($"Document with {name} not found")
        {

        }
    }
}
