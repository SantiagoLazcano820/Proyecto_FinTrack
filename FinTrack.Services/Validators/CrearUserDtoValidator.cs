using FinTrack.Core.DTOs;
using FluentValidation;

namespace FinTrack.Services.Validators
{
    public class CrearUserDtoValidator : AbstractValidator<UserDto>
    {
        public CrearUserDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).WithMessage("El ID no debe enviarse para un nuevo registro.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Debes asignar un rol válido al usuario.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio para el registro.")
                .EmailAddress().WithMessage("Formato de email inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña no puede estar vacía.")
                .MinimumLength(8).WithMessage("Por seguridad, la contraseña requiere 8 o más caracteres.");
        }
    }
}
