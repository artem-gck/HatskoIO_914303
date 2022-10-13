using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Queries.IsDocumentExit
{
    public class IsDocumentExitQueryHandler : IQueryHandler<IsDocumentExitQuery>
    {
        private readonly IDocumentRepository _documentRepository;

        public IsDocumentExitQueryHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        public async Task<IList<IResult>> Handle(IsDocumentExitQuery query)
        {
            var isExist = await _documentRepository.IsDocumentExit(query.Id);

            var documentExistDto = new DocumentExistDto()
            {
                IsExist = isExist
            };

            var documentExistList = new List<IResult>() { documentExistDto };

            return documentExistList;
        }
    }
}
