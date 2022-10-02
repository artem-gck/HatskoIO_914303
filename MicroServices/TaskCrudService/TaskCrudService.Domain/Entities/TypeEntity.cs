namespace TaskCrudService.Domain.Entities
{
    public class TypeEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<TaskEntity>? Tasks { get; set; }
    }
}
