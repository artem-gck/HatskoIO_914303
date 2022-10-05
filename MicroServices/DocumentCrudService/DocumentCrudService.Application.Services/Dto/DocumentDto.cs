using DocumentCrudService.Cqrs.Results;

namespace DocumentCrudService.Cqrs.Dto
{
    public class DocumentDto : IResult
    {
        public string Name { get; set; }
        public byte[] Body { get; set; }
    }
}
