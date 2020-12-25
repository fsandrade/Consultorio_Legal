using AutoMapper;
using CL.Core.Domain;
using CL.Core.Shared.ModelViews.Cliente;
using CL.Core.Shared.ModelViews.Endereco;
using CL.Core.Shared.ModelViews.Telefone;
using System;

namespace CL.Manager.Mappings
{
    public class NovoClienteMappingProfile : Profile
    {
        public NovoClienteMappingProfile()
        {
            CreateMap<NovoCliente, Cliente>()
                .ForMember(d => d.Criacao, o => o.MapFrom(x => DateTime.Now))
                .ForMember(d => d.DataNascimento, o => o.MapFrom(x => x.DataNascimento.Date));

            CreateMap<NovoEndereco, Endereco>();
            CreateMap<NovoTelefone, Telefone>();
            CreateMap<Cliente, ClienteView>();
            CreateMap<Endereco, EnderecoView>();
            CreateMap<Telefone, TelefoneView>();
        }
    }
}