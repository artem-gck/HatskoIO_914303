using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById
{
    public class GetDocumentByIdQuery : IQuery
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
