using FluentValidation;
using FinTrack.Core.DTOs;

namespace FinTrack.Core.Validations
{
    public class CrearUserValidator : AbstractValidator<UserDto>
    {
        public CrearUserValidator()
        {
            RuleFor(x => x.Id).Equal(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public class ActualizarUserValidator : AbstractValidator<UserDto>
    {
        public ActualizarUserValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
