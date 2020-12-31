using AutoMapper;
using CL.Core.Domain;
using CL.Core.Shared.ModelViews.Usuario;

namespace CL.Manager.Mappings
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<Usuario, UsuarioView>().ReverseMap();
        }
    }
}