using TaskCrudService.Domain.Entities;

namespace TaskCrudServiceApi.ViewModels.Responce
{
    public class DocumentResponse
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
    }
}
