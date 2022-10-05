using System.ComponentModel.DataAnnotations;

namespace TaskCrudService.ViewModels
{
    public class ArgumentViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string ArgumentType { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
