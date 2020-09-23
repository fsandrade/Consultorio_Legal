using CL.Manager.Validator;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.WebApi.Configuration
{
    public static class FluentValidationConfig
    {
        public static void AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
               .AddFluentValidation(p =>
               {
                   p.RegisterValidatorsFromAssemblyContaining<NovoClienteValidator>();
                   p.RegisterValidatorsFromAssemblyContaining<AlteraClienteValidator>();
                   p.ValidatorOptions.LanguageManager.Culture = new CultureInfo("pt-BR");
               });
        }
    }
}