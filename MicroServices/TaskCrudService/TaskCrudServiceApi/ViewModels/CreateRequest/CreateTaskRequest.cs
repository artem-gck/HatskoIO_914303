using TaskCrudServiceApi.ViewModels.Responce;

namespace TaskCrudServiceApi.ViewModels.CreateRequest
{
    public class CreateTaskRequest
    {
        public string Type { get; set; }
        public string Header { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public List<CreatePerformerRequest>? Performers { get; set; }
        public List<CreateDocumentRequest>? Documents { get; set; }
    }
}
