using CL.Core.Shared.ModelViews.Especialidade;
using FluentValidation;
using FluentValidation.Validators;

namespace CL.Manager.Validator
{
    public class ReferenciaEspecialidadeValidator : AbstractValidator<ReferenciaEspecialidade>
    {
        public ReferenciaEspecialidadeValidator()
        {
            RuleFor(p => p.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}