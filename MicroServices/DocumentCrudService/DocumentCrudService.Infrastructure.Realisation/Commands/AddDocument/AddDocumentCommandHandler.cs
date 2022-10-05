using DocumentCrudService.Cqrs.Commands;
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

        public async Task Handle(AddDocumentCommand command)
        {
            await _documentRepository.AddAsync(command.Body, command.Name);
        }
    }
}
