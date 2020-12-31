using AutoMapper;
using CL.Manager.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace CL.WebApi.Configuration
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(NovoClienteMappingProfile),
                typeof(AlteraClienteMappingProfile),
                typeof(NovoMedicoMappingProfile),
                typeof(UsuarioMappingProfile));
        }
    }
}