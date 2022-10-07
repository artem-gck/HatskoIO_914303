using System.ComponentModel.DataAnnotations;

namespace TaskCrudServiceApi.ViewModels.CreateRequest
{
    public class CreateArgumentRequest
    {
        public string ArgumentType { get; set; }
        public string Value { get; set; }
    }
}
