namespace CompanyManagementService.DataAccess.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid id)
            : base($"Not found entity with id = {id}")
        {

        }
    }
}
