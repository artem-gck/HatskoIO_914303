using FluentValidation;
using TaskCrudServiceApi.ViewModels.UpdateRequest;

namespace TaskCrudServiceApi.Validation.UpdateRequest
{
    public class UpdateArgumentRequestValidator : AbstractValidator<UpdateArgumentRequest>
    {
        public UpdateArgumentRequestValidator()
        {
            RuleFor(arg => arg.ArgumentType).NotNull()
                                            .NotEmpty();
            RuleFor(arg => arg.Value).NotNull()
                                     .NotEmpty();
        }
    }
}
