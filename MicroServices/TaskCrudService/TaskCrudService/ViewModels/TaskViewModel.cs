namespace TaskCrudService.ViewModels
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Header { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public List<ArgumentViewModel>? Arguments { get; set; }
    }
}
