using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument
{
    public class DeleteDocumentCommand : ICommand
    {
        public string Id { get; set; }
    }
}
