using FluentValidation;
using TaskCrudServiceApi.ViewModels.CreateRequest;

namespace TaskCrudServiceApi.Validation.CreateRequest
{
    public class CreateDocumentRequestValidator : AbstractValidator<CreateDocumentRequest>
    {
        public CreateDocumentRequestValidator()
        {
            RuleFor(arg => arg.DocumentId).NotNull()
                                          .NotEmpty();
        }
    }
}
