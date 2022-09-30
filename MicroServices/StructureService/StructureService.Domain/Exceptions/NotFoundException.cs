using StructureService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureService.Domain.Exceptions
{
    public class NotFoundException<T> : Exception where T : BaseEntity
    {
        public NotFoundException(int id)
            : base($"{nameof(T)} with id = {id} not found")
        {

        }
    }
}
