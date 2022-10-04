using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Infrastructure.Realisation.Commands.DeleteDocument
{
    public class DeleteDocumentCommand : ICommand
    {
        public string Id { get; set; }
    }
}
