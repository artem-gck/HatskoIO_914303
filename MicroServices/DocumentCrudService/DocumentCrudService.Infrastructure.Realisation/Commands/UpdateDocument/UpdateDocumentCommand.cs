using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument
{
    public class UpdateDocumentCommand : ICommand
    {
        public string DocumentName { get; set; }
        public byte[] DocumentData { get; set; }
    }
}
