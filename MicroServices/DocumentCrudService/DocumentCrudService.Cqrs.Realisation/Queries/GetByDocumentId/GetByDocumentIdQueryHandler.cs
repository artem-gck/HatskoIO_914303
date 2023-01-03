using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetByDocumentId
{
    public class GetByDocumentIdQueryHandler : IQueryHandler<GetByDocumentIdQuery>
    {
        private readonly IDocumentNameRepository _documentNameRepository;

        public GetByDocumentIdQueryHandler(IDocumentNameRepository documentNameRepository)
        {
            _documentNameRepository = documentNameRepository ?? throw new ArgumentNullException(nameof(documentNameRepository));
        }

        public async Task<IList<IResult>> Handle(GetByDocumentIdQuery query)
        {
            var documentName = await _documentNameRepository.GetByDocumentIdAsync(query.Id);

            var documentNameList = new List<IResult>();

            var doc = new DocumentNameDto()
            {
                Name = documentName.FileName,
                Version = documentName.Version,
                CreatorId = documentName.CreatorId,
                Id = documentName.Id,
                UploadDate = documentName.UploadDate,
            };

            documentNameList.Add(doc);

            return documentNameList;
        }
    }
}
