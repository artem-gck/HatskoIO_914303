using System.ComponentModel.DataAnnotations;

namespace DocumentCrudServiceApi.ViewModels
{
    public class UpdateDocumentRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CreaterId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
