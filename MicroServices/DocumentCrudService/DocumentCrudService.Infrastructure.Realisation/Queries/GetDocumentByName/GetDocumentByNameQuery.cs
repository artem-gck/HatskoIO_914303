using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName
{
    public class GetDocumentByNameQuery : IQuery
    {
        public string FileName { get; set; }
        public int Version { get; set; } = -1;
    }
}
