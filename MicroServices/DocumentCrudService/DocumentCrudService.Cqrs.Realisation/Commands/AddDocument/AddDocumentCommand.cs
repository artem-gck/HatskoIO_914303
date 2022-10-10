using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands.AddDocument
{
    public class AddDocumentCommand : ICommand
    {
        public string Name { get; set; }
        public byte[] Body { get; set; }
    }
}
