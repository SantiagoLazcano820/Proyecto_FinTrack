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
                .MinimumLength(8).WithMessage("La contraseña requiere 8 o más caracteres.")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe tener al menos una mayúscula.")
                .Matches(@"[0-9]").WithMessage("La contraseña debe tener al menos un número.")
                .Matches(@"[\W]").WithMessage("La contraseña debe tener al menos un carácter especial.");

            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("El apellido es obligatorio.");
        }
    }
}
