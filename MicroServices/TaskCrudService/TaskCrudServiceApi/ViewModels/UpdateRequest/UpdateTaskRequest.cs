using System.ComponentModel.DataAnnotations;
using TaskCrudServiceApi.ViewModels.Responce;

namespace TaskCrudServiceApi.ViewModels.UpdateRequest
{
    public class UpdateTaskRequest
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Header { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public List<UpdatePerformerRequest>? Performers { get; set; }
        public List<UpdateDocumentRequest>? Documents { get; set; }
    }
}
