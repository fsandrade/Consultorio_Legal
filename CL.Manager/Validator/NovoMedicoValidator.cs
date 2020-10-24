using CL.Core.Shared.ModelViews.Medico;
using CL.Manager.Interfaces.Repositories;
using FluentValidation;

namespace CL.Manager.Validator
{
    public class NovoMedicoValidator : AbstractValidator<NovoMedico>
    {
        public NovoMedicoValidator(IEspecialidadeRepository repository)
        {
            RuleFor(p => p.Nome).NotNull().NotEmpty().MaximumLength(200);

            RuleFor(p => p.CRM).NotNull().NotEmpty().GreaterThan(0);

            RuleForEach(p => p.Especialidades).SetValidator(new ReferenciaEspecialidadeValidator(repository));
        }
    }
}