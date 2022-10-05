namespace TaskCrudService.Ports.Output.Dto
{
    public class TaskDto : BaseDto
    {
        public string Type { get; set; }
        public string Header { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public List<ArgumentDto>? Arguments { get; set; }
    }
}
