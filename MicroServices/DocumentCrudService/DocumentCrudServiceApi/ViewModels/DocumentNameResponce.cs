using System.ComponentModel.DataAnnotations;

namespace DocumentCrudService.ViewModels
{
    public class DocumentNameResponce
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }
    }
}
