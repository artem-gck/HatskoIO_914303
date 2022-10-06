using System.ComponentModel.DataAnnotations;

namespace StructureServiceApi.ViewModels.Responce
{
    public class PositionResponce
    {
        public Guid Id { get; set; }

        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Name { get; set; }
    }
}
