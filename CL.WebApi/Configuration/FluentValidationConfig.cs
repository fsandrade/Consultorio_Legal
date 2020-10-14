using CL.Manager.Validator;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Globalization;

namespace CL.WebApi.Configuration
{
    public static class FluentValidationConfig
    {
        public static void AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
               .AddFluentValidation(p =>
               {
                   p.RegisterValidatorsFromAssemblyContaining<NovoClienteValidator>();
                   p.RegisterValidatorsFromAssemblyContaining<NovoEnderecoValidator>();
                   p.RegisterValidatorsFromAssemblyContaining<AlteraClienteValidator>();
                   p.ValidatorOptions.LanguageManager.Culture = new CultureInfo("pt-BR");
               });
        }
    }
}