using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class CrearCategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CrearCategoryDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).WithMessage("El ID no debe enviarse para una nueva categoría.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("La descripción no puede pasar los 100 caracteres.");
        }
    }
}
