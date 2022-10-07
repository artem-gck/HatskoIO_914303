using FluentValidation;
using TaskCrudServiceApi.ViewModels.UpdateRequest;

namespace TaskCrudServiceApi.Validation.UpdateRequest
{
    public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskRequestValidator()
        {
            RuleFor(task => task.Id).NotNull()
                                    .NotEmpty();
            RuleFor(task => task.Type).NotNull()
                          .NotEmpty();
            RuleFor(task => task.Header).NotNull()
                                        .NotEmpty();
            RuleFor(task => task.OwnerUserId).NotNull()
                                             .NotEmpty();
            RuleForEach(task => task.Arguments).SetValidator(new UpdateArgumentRequestValidator());
        }
    }
}
