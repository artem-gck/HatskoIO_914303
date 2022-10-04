using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Infrastructure.Realisation.Queries.GetNameOfDocumentById
{
    public class GetNameOfDocumentByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
