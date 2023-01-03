using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetByDocumentId
{
    public class GetByDocumentIdQuery : IQuery
    {
        public Guid Id { get; set; }
    }
}
