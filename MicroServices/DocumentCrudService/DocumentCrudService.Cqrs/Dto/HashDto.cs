using DocumentCrudService.Cqrs.Results;

namespace DocumentCrudService.Cqrs.Dto
{
    public class HashDto : IResult
    {
        public byte[] Hash { get; set; }
    }
}
