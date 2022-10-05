using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler : ICommandHandler<DeleteDocumentCommand>
    {
        private readonly IDocumentRepository _documentRepository;

        public DeleteDocumentCommandHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        public async Task Handle(DeleteDocumentCommand command)
        {
            await _documentRepository.DeleteAsync(command.Id);
        }
    }
}
