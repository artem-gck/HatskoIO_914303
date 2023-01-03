using FluentValidation;
using TaskCrudServiceApi.ViewModels.UpdateRequest;

namespace TaskCrudServiceApi.Validation.UpdateRequest
{
    public class UpdatePerformerRequestValidator : AbstractValidator<UpdatePerformerRequest>
    {
        public UpdatePerformerRequestValidator()
        {
            RuleFor(arg => arg.UserId).NotNull()
                                      .NotEmpty();
        }
    }
}
