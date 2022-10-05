using System.ComponentModel.DataAnnotations;

namespace DocumentCrudService.ViewModels
{
    public class DocumentNameViewModel
    {
        [Required]
        public string DocumentName { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }
    }
}
