using DocumentCrudService.Cqrs.Units;

namespace DocumentCrudService.Cqrs.Realisation.Commands.AddDocument
{
    public class AddDocumentCommand : ICommand
    {
        public string DocumentName { get; set; }
        public byte[] DocumentData { get; set; }
    }
}
