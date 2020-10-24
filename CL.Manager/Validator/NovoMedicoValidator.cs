using CL.Core.Shared.ModelViews.Medico;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Manager.Validator
{
    public class NovoMedicoValidator : AbstractValidator<NovoMedico>
    {
        public NovoMedicoValidator()
        {
            RuleFor(p => p.Nome).NotNull().NotEmpty().MaximumLength(200);

            RuleFor(p => p.CRM).NotNull().NotEmpty().GreaterThan(0);

            RuleForEach(p => p.Especialidades).SetValidator(new ReferenciaEspecialidadeValidator());
        }
    }
}