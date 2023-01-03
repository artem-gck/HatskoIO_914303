using FluentValidation;
using TaskCrudServiceApi.ViewModels.CreateRequest;

namespace TaskCrudServiceApi.Validation.CreateRequest
{
    public class CreatePerformerRequestValidator : AbstractValidator<CreatePerformerRequest>
    {
        public CreatePerformerRequestValidator()
        {
            RuleFor(arg => arg.UserId).NotNull()
                                      .NotEmpty();
        }
    }
}
