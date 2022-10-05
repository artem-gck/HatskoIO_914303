using System.Text.Json.Serialization;

namespace TaskCrudService.Domain.Entities
{
    public class ArgumentEntity : BaseEntity
    {
        public ArgumentTypeEntity ArgumentType { get; set; }
        public string Value { get; set; }

        [JsonIgnore]
        public TaskEntity Task { get; set; }
    }
}
