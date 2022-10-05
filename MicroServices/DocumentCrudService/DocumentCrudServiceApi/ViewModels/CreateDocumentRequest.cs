using System.ComponentModel.DataAnnotations;

namespace DocumentCrudService.ViewModels
{
    public class CreateDocumentRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
