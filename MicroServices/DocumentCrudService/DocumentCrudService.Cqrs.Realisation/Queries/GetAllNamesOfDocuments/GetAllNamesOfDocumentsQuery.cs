using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments
{
    public class GetAllNamesOfDocumentsQuery : IQuery
    {
        public int NumberOfPage { get; set; }
        public int CountOnPage { get; set; }
    }
}
