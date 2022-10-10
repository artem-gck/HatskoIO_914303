using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById
{
    public class GetDocumentByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
