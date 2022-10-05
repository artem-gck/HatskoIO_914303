using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName
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

            var documentDto = new DocumentDto() 
            {
                FileName = document.FileName,
                DocumentBody = document.File
            };

            var documentList = new List<IResult>() { documentDto };

            return documentList;
        }
    }
}
