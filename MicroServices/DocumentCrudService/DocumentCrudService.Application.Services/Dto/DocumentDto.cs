using DocumentCrudService.Application.Services.Results;

namespace DocumentCrudService.Application.Services.Dto
{
    public class DocumentDto : IResult
    {
        public byte[] DocumentBody { get; set; }
    }
}
