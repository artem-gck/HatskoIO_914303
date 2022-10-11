using System.ComponentModel.DataAnnotations;

namespace DocumentCrudService.ViewModels
{
    public class DocumentNameResponce
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
