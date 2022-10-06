using System.ComponentModel.DataAnnotations;
using TaskCrudServiceApi.ViewModels.Responce;

namespace TaskCrudServiceApi.ViewModels.UpdateRequest
{
    public class UpdateTaskRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Header { get; set; }

        [Required]
        public Guid OwnerUserId { get; set; }

        [Required]
        public DateTime DeadLine { get; set; }
        public List<UpdateArgumentRequest>? Arguments { get; set; }
    }
}
