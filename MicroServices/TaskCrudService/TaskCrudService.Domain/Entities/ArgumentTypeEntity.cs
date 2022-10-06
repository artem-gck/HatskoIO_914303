using System.Text.Json.Serialization;

namespace TaskCrudService.Domain.Entities
{
    public class ArgumentTypeEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<ArgumentEntity>? Arguments { get; set; }
    }
}
