using System.ComponentModel.DataAnnotations;

namespace TaskCrudService.ViewModels
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Header { get; set; }

        [Required]
        public Guid OwnerUserId { get; set; }

        public DateTime DeadLine { get; set; }

        public List<ArgumentViewModel>? Arguments { get; set; }
    }
}
