using System.Text.Json.Serialization;

namespace TaskCrudService.Domain.Entities
{
    public class TypeEntity : BaseEntity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public List<TaskEntity>? Tasks { get; set; }
    }
}
