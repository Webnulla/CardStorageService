using CardStorageService.Models.Requests;
using FluentValidation;

namespace CardStorageService.Models.Validator;

public class CardValidator : AbstractValidator<CreateCardRequest>
{
    public CardValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Length(5, 50);
        RuleFor(x => x.CardNo)
            .Length(10);
    }
}