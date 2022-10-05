using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument
{
    public class UpdateDocumentCommand : ICommand
    {
        public string Name { get; set; }
        public byte[] Body { get; set; }
    }
}
