using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Results;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument
{
    public class UpdateDocumentCommandHandler : ICommandHandler<UpdateDocumentCommand>
    {
        private readonly IDocumentRepository _documentRepository;

        public UpdateDocumentCommandHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        public async Task<IResult> Handle(UpdateDocumentCommand command)
        {
            await _documentRepository.UpdateAsync(command.Id, command.CreaterId, command.Body, command.Name);

            return null;
        }
    }
}
