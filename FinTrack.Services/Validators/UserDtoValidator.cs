using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("Debe ingresar un formato de correo electrónico verídico.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres para ser segura.");
        }
    }
}