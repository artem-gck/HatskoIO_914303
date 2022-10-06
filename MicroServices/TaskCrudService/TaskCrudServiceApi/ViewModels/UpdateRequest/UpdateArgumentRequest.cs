using System.ComponentModel.DataAnnotations;

namespace TaskCrudServiceApi.ViewModels.UpdateRequest
{
    public class UpdateArgumentRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ArgumentType { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
