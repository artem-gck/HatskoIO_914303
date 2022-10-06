using System.ComponentModel.DataAnnotations;

namespace StructureServiceApi.ViewModels.Responce
{
    public class DepartmentUnitResponce
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
