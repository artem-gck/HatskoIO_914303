using FluentValidation;
using TaskCrudServiceApi.ViewModels.UpdateRequest;

namespace TaskCrudServiceApi.Validation.UpdateRequest
{
    public class UpdateDocumentRequestValidator : AbstractValidator<UpdateDocumentRequest>
    {
        public UpdateDocumentRequestValidator()
        {
            RuleFor(arg => arg.DocumentId).NotNull()
                                          .NotEmpty();
        }
    }
}
