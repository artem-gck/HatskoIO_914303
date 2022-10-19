namespace CompanyManagementService.DataAccess.Exceptions
{
    public class DbUpdateException : Exception
    {
        public DbUpdateException()
            : base("Conflict with db update")
        {

        }
    }
}
