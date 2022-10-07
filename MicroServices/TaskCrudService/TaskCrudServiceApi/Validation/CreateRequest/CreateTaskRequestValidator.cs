using FluentValidation;
using TaskCrudServiceApi.ViewModels.CreateRequest;

namespace TaskCrudServiceApi.Validation.CreateRequest
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(task => task.Type).NotNull()
                                      .NotEmpty();
            RuleFor(task => task.Header).NotNull()
                                        .NotEmpty();
            RuleFor(task => task.OwnerUserId).NotNull()
                                             .NotEmpty();
            RuleForEach(task => task.Arguments).SetValidator(new CreateArgumentRequestValidator());
        }
    }
}
