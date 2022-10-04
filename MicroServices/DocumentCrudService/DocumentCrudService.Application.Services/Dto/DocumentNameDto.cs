using DocumentCrudService.Application.Services.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentCrudService.Application.Services.Dto
{
    public class DocumentNameDto : IResult
    {
        public string DocumentName { get; set; }
        public string Id { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
