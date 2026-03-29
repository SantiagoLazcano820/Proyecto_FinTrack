using FluentValidation;
using FinTrack.Core.DTOs;

namespace FinTrack.Core.Validations
{
    public class CrearTransactionValidator : AbstractValidator<TransactionDto>
    {
        public CrearTransactionValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).WithMessage("El ID debe ser 0 para la creación.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("El ID de categoría es obligatorio.");

            RuleFor(t => t.Amount)
                .GreaterThan(0).WithMessage("El monto debe ser positivo.");

            RuleFor(t => t.Type)
                .NotEmpty().WithMessage("El tipo es obligatorio.")
                .Must(x => x == "Income" || x == "Expense").WithMessage("El tipo debe ser 'Income' o 'Expense'.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("Formato de fecha inválido.")
                .Must(BeNotFutureDate).WithMessage("La fecha no puede ser futura.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MinimumLength(5).WithMessage("Mínimo 5 caracteres.")
                .MaximumLength(500).WithMessage("Máximo 500 caracteres.");
        }

        private bool BeAValidDate(string date) => DateTime.TryParse(date, out _);
        private bool BeNotFutureDate(string date) => DateTime.TryParse(date, out var d) && d <= DateTime.Now;
    }

    public class ActualizarTransactionValidator : AbstractValidator<TransactionDto>
    {
        public ActualizarTransactionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID es obligatorio para actualizar.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("El ID de categoría es obligatorio.");

            RuleFor(t => t.Amount)
                .GreaterThan(0).WithMessage("El monto debe ser positivo.");

            RuleFor(t => t.Type)
                .NotEmpty().WithMessage("El tipo es obligatorio.")
                .Must(x => x == "Income" || x == "Expense").WithMessage("El tipo debe ser 'Income' o 'Expense'.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("Formato de fecha inválido.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MinimumLength(5).WithMessage("Mínimo 5 caracteres.")
                .MaximumLength(500).WithMessage("Máximo 500 caracteres.");
        }

        private bool BeAValidDate(string date) => DateTime.TryParse(date, out _);
    }
}
