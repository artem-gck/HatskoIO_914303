using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetHashOfDocument
{
    public class GetHashOfDocumentQuery : IQuery
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
