namespace StructureService.Domain.Exceptions
{
    public class PositionNotFoundException : Exception
    {
        public PositionNotFoundException(int id)
            : base($"Position with id = {id} not found")
        {

        }
    }
}
