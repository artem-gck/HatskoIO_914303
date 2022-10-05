using System.ComponentModel.DataAnnotations;

namespace DocumentCrudService.ViewModels
{
    public class DocumentViewModel
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
