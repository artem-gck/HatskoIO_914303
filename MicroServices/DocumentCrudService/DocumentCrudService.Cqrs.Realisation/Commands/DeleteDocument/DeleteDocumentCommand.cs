using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument
{
    public class DeleteDocumentCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
