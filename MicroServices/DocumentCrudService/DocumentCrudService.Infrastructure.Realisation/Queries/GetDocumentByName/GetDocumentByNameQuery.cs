using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentByName
{
    public class GetDocumentByNameQuery : IQuery
    {
        public string FileName { get; set; }
        public int Version { get; set; } = -1;
    }
}
