using DocumentCrudService.Cqrs.Results;

namespace DocumentCrudService.Cqrs.Dto
{
    public class DocumentDto : IResult
    {
        public string FileName { get; set; }
        public byte[] DocumentBody { get; set; }
    }
}
