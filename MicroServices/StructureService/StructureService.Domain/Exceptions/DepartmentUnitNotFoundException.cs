namespace StructureService.Domain.Exceptions
{
    public class DepartmentUnitNotFoundException : Exception
    {
        public DepartmentUnitNotFoundException(int id)
            : base($"Department unit with id = {id} not found")
        {

        }
    }
}
