using System.ComponentModel.DataAnnotations;

namespace DocumentCrudService.ViewModels
{
    public class CreateDocumentRequest
    {
        [Required]
        public Guid CreaterId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
