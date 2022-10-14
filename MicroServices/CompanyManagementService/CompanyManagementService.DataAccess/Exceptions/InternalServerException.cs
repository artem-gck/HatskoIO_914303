namespace CompanyManagementService.DataAccess.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException(string service)
            : base($"Internal {service} server")
        {

        }
    }
}
