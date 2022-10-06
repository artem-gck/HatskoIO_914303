using System.ComponentModel.DataAnnotations;

namespace StructureServiceApi.ViewModels.Responce
{
    public class DepartmentResponce
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
