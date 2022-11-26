using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocumentsByUserId
{
    public class GetAllNamesOfDocumentsByUserIdQuery : IQuery
    {
        public Guid CreatorId { get; set; }
    }
}
