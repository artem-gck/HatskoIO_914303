using System.ComponentModel.DataAnnotations;
using IResult = DocumentCrudService.Application.Services.Results.IResult;

namespace DocumentCrudService.ViewModels
{
    public class DocumentNameViewModel : IResult
    {
        [Required]
        public string DocumentName { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }
    }
}
