using FluentValidation;
namespace DataClash.Application.PlayerCards.Commands.UpdatePlayerCard
{
    public class UpdatePlayerCardCommandValidator : AbstractValidator<UpdatePlayerCardCommand>
    {
        public UpdatePlayerCardCommandValidator()
        {
            RuleFor(v => v.CardId).NotEmpty();
            RuleFor(v => v.PlayerId).NotEmpty();
            RuleFor(v => v.Level).NotEmpty();
        }
    }
}