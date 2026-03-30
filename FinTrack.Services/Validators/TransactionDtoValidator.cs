using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class TransactionDtoValidator : AbstractValidator<TransactionDto>
    {
        public TransactionDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id > 0)
                .WithMessage("El ID de la transacción debe ser mayor que cero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio.")
                .NotEmpty().WithMessage("El ID de usuario no puede estar vacío.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("El monto de la transacción debe ser mayor a 0.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("La fecha debe tener un formato válido (dd-MM-yyyy).")
                .Must(BeNotFutureDate).WithMessage("No puedes registrar transacciones futuras.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("El tipo ('Income' o 'Expense') es obligatorio.")
                .Must(x => x == "Income" || x == "Expense").WithMessage("El tipo debe ser 'Income' (Ingreso) o 'Expense' (Gasto).");
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        } 

        private bool BeNotFutureDate(string date)
        {
            if (DateTime.TryParse(date, out var parsedDate))
            {
                return parsedDate <= DateTime.Now;
            }
            return true;
        }
    }
}
