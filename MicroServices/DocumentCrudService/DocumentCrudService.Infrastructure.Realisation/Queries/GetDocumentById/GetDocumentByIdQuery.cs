using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentById
{
    public class GetDocumentByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
