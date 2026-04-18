using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class ActualizarUserDtoValidator : AbstractValidator<UserDto>
    {
        public ActualizarUserDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del usuario es obligatorio para actualizar.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre no puede quedar vacío.")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido no puede quedar vacío.")
                .MaximumLength(50);
        }
    }
}