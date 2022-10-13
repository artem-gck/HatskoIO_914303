namespace DocumentCrudService.Repositories.Exceptions
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string name)
            : base($"Document with {name} not in data base found")
        {

        }
    }
}
