using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument
{
    public class UpdateDocumentCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid CreaterId { get; set; }
        public string Name { get; set; }
        public byte[] Body { get; set; }
    }
}
