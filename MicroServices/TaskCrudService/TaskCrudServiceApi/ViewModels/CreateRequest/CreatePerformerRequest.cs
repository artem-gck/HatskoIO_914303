namespace TaskCrudServiceApi.ViewModels.CreateRequest
{
    public class CreatePerformerRequest
    {
        public Guid UserId { get; set; }
        public string? TypeOfTask { get; set; }
        public string? Description { get; set; }
    }
}
