using IResult = DocumentCrudService.Application.Services.Results.IResult;

namespace DocumentCrudService.ViewModels
{
    public class DocumentNameViewModel : IResult
    {
        public string DocumentName { get; set; }
        public string Id { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
