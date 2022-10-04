using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Infrastructure.Realisation.Commands.UpdateDocument
{
    public class UpdateDocumentCommand : ICommand
    {
        public string DocumentName { get; set; }
        public byte[] DocumentData { get; set; }
    }
}
