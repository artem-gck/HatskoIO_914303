namespace CompanyManagementService.DataAccess.Exceptions
{
    public class InvalidModelStateException : Exception
    {
        public InvalidModelStateException(string modelName)
            : base($"Invalid model state of {modelName}")
        {

        }
    }
}
