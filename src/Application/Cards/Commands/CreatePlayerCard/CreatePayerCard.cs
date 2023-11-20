
using FluentValidation;

namespace DataClash.Application.PlayerCards.Commands.CreatePlayerCard
{
    public class CreatePlayerCardCommandValidator : AbstractValidator<CreatePlayerCardCommand>
    {
        public CreatePlayerCardCommandValidator()
        {
            RuleFor(v => v.CardId).NotEmpty();
            RuleFor(v => v.PlayerId).NotEmpty();
            RuleFor(v => v.Level).NotEmpty();
        }
    }
}
