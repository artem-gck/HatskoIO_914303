using DocumentCrudService.Cqrs.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentCrudService.Cqrs.Dto
{
    public class IdDto : IResult
    {
        public string? Id { get; set; }
    }
}
