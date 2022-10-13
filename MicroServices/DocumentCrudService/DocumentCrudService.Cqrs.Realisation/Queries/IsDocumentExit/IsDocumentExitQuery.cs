using DocumentCrudService.Cqrs.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentCrudService.Cqrs.Realisation.Queries.IsDocumentExit
{
    public class IsDocumentExitQuery : IQuery
    {
        public Guid Id { get; set; }
    }
}
