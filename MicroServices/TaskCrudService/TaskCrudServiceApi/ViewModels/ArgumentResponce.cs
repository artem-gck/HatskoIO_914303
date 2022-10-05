using System.ComponentModel.DataAnnotations;

namespace TaskCrudServiceApi.ViewModels
{
    public class ArgumentResponce
    {
        public Guid Id { get; set; }
        public string ArgumentType { get; set; }
        public string Value { get; set; }
    }
}
