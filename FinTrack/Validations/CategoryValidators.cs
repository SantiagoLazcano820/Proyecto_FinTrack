using FluentValidation;
using FinTrack.Core.DTOs;

namespace FinTrack.Core.Validations
{
    public class CrearCategoryValidator : AbstractValidator<CategoryDto>
    {
        public CrearCategoryValidator()
        {
            RuleFor(x => x.Id).Equal(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }

    public class ActualizarCategoryValidator : AbstractValidator<CategoryDto>
    {
        public ActualizarCategoryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
