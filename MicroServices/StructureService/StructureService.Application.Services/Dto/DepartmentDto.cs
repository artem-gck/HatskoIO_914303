using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureService.Application.Services.Dto
{
    public class DepartmentDto : BaseDto
    {
        public string Name { get; set; }
        public int CheifUserId { get; set; }
    }
}
