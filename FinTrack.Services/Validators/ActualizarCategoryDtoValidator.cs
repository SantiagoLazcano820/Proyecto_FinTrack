using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class ActualizarCategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public ActualizarCategoryDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID de la categoría es obligatorio para actualizar.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre no puede estar vacío.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }
}
