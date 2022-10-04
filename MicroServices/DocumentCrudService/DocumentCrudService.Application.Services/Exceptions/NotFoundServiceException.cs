using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentCrudService.Application.Services.Exceptions
{
    public class NotFoundServiceException : Exception
    {
        public NotFoundServiceException() : base()
        {
        }

        public NotFoundServiceException(string message) 
            : base(message)
        {
        }

        public NotFoundServiceException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        public NotFoundServiceException(string name, object key) 
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
