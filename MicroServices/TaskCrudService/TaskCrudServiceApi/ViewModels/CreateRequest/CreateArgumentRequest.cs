using System.ComponentModel.DataAnnotations;

namespace TaskCrudServiceApi.ViewModels.CreateRequest
{
    public class CreateArgumentRequest
    {
        [Required]
        public string ArgumentType { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
