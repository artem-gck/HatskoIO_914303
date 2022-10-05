using System.Text.Json.Serialization;

namespace TaskCrudService.Domain.Entities
{
    public class ArgumentTypeEntity : BaseEntity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public List<ArgumentEntity>? Arguments { get; set; }
    }
}
