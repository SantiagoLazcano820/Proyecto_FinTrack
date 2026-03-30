using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class CrearTransactionDtoValidator : AbstractValidator<TransactionDto>
    {
        public CrearTransactionDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).WithMessage("El ID no debe enviarse para crear una nueva transacción.");

            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("ID de usuario inválido.");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Debes seleccionar una categoría válida.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("El monto debe ser positivo.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("Formato de fecha inválido.");
        }

        private bool BeAValidDate(string date) => DateTime.TryParse(date, out _);
    }
}
