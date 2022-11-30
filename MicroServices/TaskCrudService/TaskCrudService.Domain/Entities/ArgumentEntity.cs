using System.Text.Json.Serialization;

namespace TaskCrudService.Domain.Entities
{
    public class ArgumentEntity : BaseEntity
    {
        public Guid ArgumentTypeId { get; set; }
        public ArgumentTypeEntity ArgumentType { get; set; }
        public string Value { get; set; }
        public Guid TaskId { get; set; }
        public TaskEntity Task { get; set; }
    }
}
