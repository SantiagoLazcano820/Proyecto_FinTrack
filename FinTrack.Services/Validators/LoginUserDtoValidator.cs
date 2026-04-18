using FinTrack.Core.DTOs;
using FluentValidation;

public class LoginUserDtoValidator : AbstractValidator<UserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El usuario y la contraseña son obligatorios.")
            .EmailAddress().WithMessage("Formato de correo no válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("El usuario y la contraseña son obligatorios.");
    }
}
