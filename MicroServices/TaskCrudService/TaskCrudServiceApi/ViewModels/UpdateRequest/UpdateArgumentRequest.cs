using System.ComponentModel.DataAnnotations;

namespace TaskCrudServiceApi.ViewModels.UpdateRequest
{
    public class UpdateArgumentRequest
    {
        public Guid Id { get; set; }
        public string ArgumentType { get; set; }
        public string Value { get; set; }
    }
}
