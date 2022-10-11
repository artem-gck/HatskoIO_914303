using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Commands.AddDocument
{
    public class AddDocumentCommandHandler : ICommandHandler<AddDocumentCommand>
    {
        private readonly IDocumentRepository _documentRepository;

        public AddDocumentCommandHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        public async Task<IResult> Handle(AddDocumentCommand command)
        {
            var id = await _documentRepository.AddAsync(command.CreaterId, command.Body, command.Name);

            return new IdDto() { Id = id };
        }
    }
}
