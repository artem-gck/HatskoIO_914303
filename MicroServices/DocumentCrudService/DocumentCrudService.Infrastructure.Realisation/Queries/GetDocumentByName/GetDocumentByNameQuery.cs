using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName
{
    public class GetDocumentByNameQuery : IQuery
    {
        public string Name { get; set; }
        public int Version { get; set; } = -1;
    }
}
