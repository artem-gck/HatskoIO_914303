namespace StructureService.Domain.Exceptions
{
    public class DepartmentNotFoundException : Exception
    {
        public DepartmentNotFoundException(int id)
            : base($"Department with id = {id} not found")
        {

        }
    }
}
