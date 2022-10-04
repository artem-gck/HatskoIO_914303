using DocumentCrudService.Application.DbServices;
using DocumentCrudService.Application.Services.Dto;
using DocumentCrudService.Application.Services.Queries;
using DocumentCrudService.Application.Services.Results;

namespace DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentById
{
    public class GetDocumentByIdQueryHandler : IQueryHandler<GetDocumentByIdQuery>
    {
        private readonly IDocumentRepository _documentRepository;

        public GetDocumentByIdQueryHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        public async Task<IList<IResult>> Handle(GetDocumentByIdQuery query)
        {
            var document = await _documentRepository.GetAsync(query.Id);

            var documentDto = new DocumentDto() { DocumentBody = document };

            var documentList = new List<IResult>() { documentDto };

            return documentList;
        }
    }
}
