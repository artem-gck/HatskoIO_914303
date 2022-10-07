using FluentValidation;
using TaskCrudServiceApi.ViewModels.CreateRequest;

namespace TaskCrudServiceApi.Validation.CreateRequest
{
    public class CreateArgumentRequestValidator : AbstractValidator<CreateArgumentRequest>
    {
        public CreateArgumentRequestValidator()
        {
            RuleFor(arg => arg.ArgumentType).NotNull()
                                            .NotEmpty();
            RuleFor(arg => arg.Value).NotNull()
                                     .NotEmpty();
        }
    }
}
