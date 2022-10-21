using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetHashOfDocument
{
    public class GetHashOfDocumentQueryHandler : IQueryHandler<GetHashOfDocumentQuery>
    {
        private readonly IDocumentRepository _documentRepository;

        public GetHashOfDocumentQueryHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        public async Task<IList<IResult>> Handle(GetHashOfDocumentQuery query)
        {
            var document = await _documentRepository.GetAsync(query.Id, query.Version);

            var hashDto = new HashDto()
            {
                Hash = document.File.GetMD5Hash()
            };

            var hashList = new List<IResult>() { hashDto };

            return hashList;
        }
    }
}
