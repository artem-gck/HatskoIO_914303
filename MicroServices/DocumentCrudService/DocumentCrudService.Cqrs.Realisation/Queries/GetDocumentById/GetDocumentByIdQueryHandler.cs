using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById
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

            var documentDto = new DocumentDto()
            {
                Name = document.FileName,
                Body = document.File 
            };

            var documentList = new List<IResult>() { documentDto };

            return documentList;
        }
    }
}
