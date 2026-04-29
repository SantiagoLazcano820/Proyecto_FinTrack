using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class MonthlyBalanceQueryValidator : AbstractValidator<(int month, int year)>
    {
        public MonthlyBalanceQueryValidator()
        {
            RuleFor(x => x.month)
                .InclusiveBetween(1, 12)
                .WithMessage("El mes debe estar entre 1 y 12.");

            RuleFor(x => x.year)
                .InclusiveBetween(2000, 2100)
                .WithMessage("El año debe ser un valor válido.");
        }
    }
}
