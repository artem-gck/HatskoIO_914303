using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocumentsByUserId
{
    public class GetAllNamesOfDocumentsByUserIdQueryHanler : IQueryHandler<GetAllNamesOfDocumentsByUserIdQuery>
    {
        private readonly IDocumentNameRepository _documentNameRepository;

        public GetAllNamesOfDocumentsByUserIdQueryHanler(IDocumentNameRepository documentNameRepository)
        {
            _documentNameRepository = documentNameRepository ?? throw new ArgumentNullException(nameof(documentNameRepository));
        }

        public async Task<IList<IResult>> Handle(GetAllNamesOfDocumentsByUserIdQuery query)
        {
            var listOfDocumentName = await _documentNameRepository.GetByUserIdAsync(query.CreatorId, query.Count);

            var documentNameList = new List<IResult>();

            foreach (var document in listOfDocumentName)
            {
                var doc = new DocumentNameDto()
                {
                    Name = document.FileName,
                    Version = document.Version,
                    CreatorId = document.CreatorId,
                    Id = document.Id,
                    UploadDate = document.UploadDate,
                };

                documentNameList.Add(doc);
            }

            return documentNameList;
        }
    }
}
