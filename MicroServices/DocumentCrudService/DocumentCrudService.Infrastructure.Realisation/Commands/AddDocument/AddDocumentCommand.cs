using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Infrastructure.Realisation.Commands.AddDocument
{
    public class AddDocumentCommand : ICommand
    {
        public string DocumentName { get; set; }
        public byte[] DocumentData { get; set; }
    }
}
