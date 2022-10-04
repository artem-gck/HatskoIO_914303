using DocumentCrudService.Application.DbServices;
using DocumentCrudService.Application.Services.Dto;
using DocumentCrudService.Application.Services.Queries;
using DocumentCrudService.Application.Services.Results;

namespace DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentByName
{
    public class GetDocumentByNameQueryHandler : IQueryHandler<GetDocumentByNameQuery>
    {
        private readonly IDocumentNameRepository _doucmentNameRepository;

        public GetDocumentByNameQueryHandler(IDocumentNameRepository doucmentNameRepository)
        {
            _doucmentNameRepository = doucmentNameRepository ?? throw new ArgumentNullException(nameof(doucmentNameRepository));
        }

        public async Task<IList<IResult>> Handle(GetDocumentByNameQuery query)
        {
            var document = await _doucmentNameRepository.GetByNameAsync(query.FileName, query.Version);

            var documentDto = new DocumentDto() { DocumentBody = document };

            var documentList = new List<IResult>() { documentDto };

            return documentList;
        }
    }
}
