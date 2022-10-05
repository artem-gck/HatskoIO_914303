using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments
{
    public class GetAllNamesOfDocumentsQueryHandler : IQueryHandler<GetAllNamesOfDocumentsQuery>
    {
        private readonly IDocumentNameRepository _documentNameRepository;

        public GetAllNamesOfDocumentsQueryHandler(IDocumentNameRepository documentNameRepository)
        {
            _documentNameRepository = documentNameRepository ?? throw new ArgumentNullException(nameof(documentNameRepository));
        }

        public async Task<IList<IResult>> Handle(GetAllNamesOfDocumentsQuery query)
        {
            var listOfDocumentName = await _documentNameRepository.GetAllAsync();

            var documentNameList = new List<IResult>();

            foreach (var document in listOfDocumentName)
            {
                var doc = new DocumentNameDto()
                {
                    DocumentName = document.FileName,
                    Id = document.Id,
                    UploadDate = document.UploadDate,
                };

                documentNameList.Add(doc);
            }

            return documentNameList;
        }
    }
}
