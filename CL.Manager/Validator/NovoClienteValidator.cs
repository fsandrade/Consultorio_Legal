using CL.Core.Shared.ModelViews;
using FluentValidation;
using System;

namespace CL.Manager.Validator
{
    public class NovoClienteValidator : AbstractValidator<NovoCliente>
    {
        public NovoClienteValidator()
        {
            RuleFor(x => x.Nome).NotNull().NotEmpty().MinimumLength(10).MaximumLength(150);
            RuleFor(x => x.DataNascimento).NotNull().NotEmpty().LessThan(DateTime.Now).GreaterThan(DateTime.Now.AddYears(-130));
            RuleFor(x => x.Documento).NotNull().NotEmpty().MinimumLength(4).MaximumLength(14);
            RuleFor(x => x.Telefone).NotNull().NotEmpty().Matches("[1-9][0-9]{10}").WithMessage("O telefone tem que ter o formato [2-9][0-9]{10}");
            RuleFor(x => x.Sexo).NotNull().NotEmpty().Must(IsMorF).WithMessage("Sexo precisa ser M ou F");
            RuleFor(x => x.Endereco).SetValidator(new NovoEnderecoValidator());
        }

        private bool IsMorF(char sexo)
        {
            return sexo == 'M' || sexo == 'F';
        }
    }
}