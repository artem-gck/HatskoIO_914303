using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName
{
    public class GetDocumentByNameQueryHandler : IQueryHandler<GetDocumentByNameQuery>
    {
        private readonly IDocumentRepository _doucmentRepository;

        public GetDocumentByNameQueryHandler(IDocumentRepository doucmentRepository)
        {
            _doucmentRepository = doucmentRepository ?? throw new ArgumentNullException(nameof(doucmentRepository));
        }

        public async Task<IList<IResult>> Handle(GetDocumentByNameQuery query)
        {
            var document = await _doucmentRepository.GetByNameAsync(query.Name, query.Version);

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
