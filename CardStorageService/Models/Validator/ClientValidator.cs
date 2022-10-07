using CardStorageService.Models.Requests;
using FluentValidation;

namespace CardStorageService.Models.Validator;

public class ClientValidator : AbstractValidator<CreateClientRequest>
{
    public ClientValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .Length(1, 50);
        RuleFor(x => x.Patronymc)
            .NotNull()
            .Length(1, 50);
        RuleFor(x => x.Surname)
            .NotNull()
            .Length(1, 50);
    }
}