using TaskCrudServiceApi.ViewModels.Responce;

namespace TaskCrudServiceApi.ViewModels.CreateRequest
{
    public class CreateTaskRequest
    {
        public string Type { get; set; }
        public string Header { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public List<ArgumentResponce>? Arguments { get; set; }
    }
}
