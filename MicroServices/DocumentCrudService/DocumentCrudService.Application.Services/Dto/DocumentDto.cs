using DocumentCrudService.Application.Services.Results;

namespace DocumentCrudService.Application.Services.Dto
{
    public class DocumentDto : IResult
    {
        public string FileName { get; set; }
        public byte[] DocumentBody { get; set; }
    }
}
