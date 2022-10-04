using DocumentCrudService.Application.DbServices;
using DocumentCrudService.Application.Services.Dto;
using DocumentCrudService.Application.Services.Handlers;
using DocumentCrudService.Application.Services.Results;

namespace DocumentCrudService.Infrastructure.Realisation.Queries.GetNameOfDocumentById
{
    public class GetNameOfDocumentByIdQueryHandler : IQueryHandler<GetNameOfDocumentByIdQuery>
    {
        private readonly IDocumentNameRepository _doucmentNameRepository;

        public GetNameOfDocumentByIdQueryHandler(IDocumentNameRepository doucmentNameRepository)
        {
            _doucmentNameRepository = doucmentNameRepository ?? throw new ArgumentNullException(nameof(doucmentNameRepository));
        }

        public async Task<IList<IResult>> Handle(GetNameOfDocumentByIdQuery query)
        {
            var documentName = await _doucmentNameRepository.GetAsync(query.Id);

            var documentDto = new DocumentNameDto() 
            { 
                DocumentName = documentName.FileName,
                Id = documentName.Id,
                UploadDate = documentName.UploadDate,
            };

            var documentNameList = new List<IResult>() { documentDto };

            return documentNameList;
        }
    }
}
