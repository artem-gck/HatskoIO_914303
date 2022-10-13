using DocumentCrudService.Cqrs.Results;

namespace DocumentCrudService.Cqrs.Dto
{
    public class DocumentExistDto : IResult
    {
        public bool IsExist { get; set; }
    }
}
