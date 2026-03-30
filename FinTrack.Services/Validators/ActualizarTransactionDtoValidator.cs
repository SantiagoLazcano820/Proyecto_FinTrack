using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class ActualizarTransactionDtoValidator : AbstractValidator<TransactionDto>
    {
        public ActualizarTransactionDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID de la transacción es obligatorio para actualizar.");

            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("ID de usuario obligatorio.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("Formato de fecha inválido (dd-MM-yyyy).")
                .Must(BeNotFutureDate).WithMessage("La fecha no puede ser futura.");
        }

        private bool BeAValidDate(string date) => DateTime.TryParse(date, out _);

        private bool BeNotFutureDate(string date)
        {
            if (DateTime.TryParse(date, out var parsedDate))
                return parsedDate <= DateTime.Now;
            return true;
        }
    }
}
