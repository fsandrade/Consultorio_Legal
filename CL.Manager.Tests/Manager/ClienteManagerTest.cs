using AutoMapper;
using CL.Core.Domain;
using CL.Core.Shared.ModelViews.Cliente;
using CL.FakeData.ClienteData;
using CL.Manager.Implementation;
using CL.Manager.Interfaces.Managers;
using CL.Manager.Interfaces.Repositories;
using CL.Manager.Mappings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CL.Manager.Tests.Manager
{
    public class ClienteManagerTest
    {
        private readonly IClienteRepository repository;
        private readonly ILogger<ClienteManager> logger;
        private readonly IMapper mapper;
        private readonly IClienteManager manager;
        private readonly Cliente Cliente;
        private readonly NovoCliente NovoCliente;
        private readonly AlteraCliente AlteraCliente;
        private readonly ClienteFaker ClienteFaker;
        private readonly NovoClienteFaker NovoClienteFaker;
        private readonly AlteraClienteFaker AlteraClienteFaker;

        public ClienteManagerTest()
        {
            repository = Substitute.For<IClienteRepository>();
            logger = Substitute.For<ILogger<ClienteManager>>();
            mapper = new MapperConfiguration(p => p.AddProfile<NovoClienteMappingProfile>()).CreateMapper();
            manager = new ClienteManager(repository, mapper, logger);
            ClienteFaker = new ClienteFaker();
            NovoClienteFaker = new NovoClienteFaker();
            AlteraClienteFaker = new AlteraClienteFaker();

            Cliente = ClienteFaker.Generate();
            NovoCliente = NovoClienteFaker.Generate();
            AlteraCliente = AlteraClienteFaker.Generate();
        }

        [Fact]
        public async Task GetClientesAsync_Sucesso()
        {
            var listaClientes = ClienteFaker.Generate(10);
            repository.GetClientesAsync().Returns(listaClientes);
            var controle = mapper.Map<IEnumerable<Cliente>, IEnumerable<ClienteView>>(listaClientes);
            var retorno = await manager.GetClientesAsync();

            await repository.Received().GetClientesAsync();
            retorno.Should().BeEquivalentTo(controle);
        }

        [Fact]
        public async Task GetClientesAsync_Vazio()
        {
            repository.GetClientesAsync().Returns(new List<Cliente>());

            var retorno = await manager.GetClientesAsync();

            await repository.Received().GetClientesAsync();
            retorno.Should().BeEquivalentTo(new List<Cliente>());
        }

        [Fact]
        public async Task GetClienteAsync_Sucesso()
        {
            repository.GetClienteAsync(Arg.Any<int>()).Returns(Cliente);
            var controle = mapper.Map<ClienteView>(Cliente);
            var retorno = await manager.GetClienteAsync(Cliente.Id);

            await repository.Received().GetClienteAsync(Arg.Any<int>());
            retorno.Should().BeEquivalentTo(controle);
        }

        [Fact]
        public async Task GetClienteAsync_NaoEncontrado()
        {
            repository.GetClienteAsync(Arg.Any<int>()).Returns(new Cliente());
            var controle = mapper.Map<ClienteView>(new Cliente());
            var retorno = await manager.GetClienteAsync(1);

            await repository.Received().GetClienteAsync(Arg.Any<int>());
            retorno.Should().BeEquivalentTo(controle);
        }

        [Fact]
        public async Task InsertClienteAsync_Sucesso()
        {
            repository.InsertClienteAsync(Arg.Any<Cliente>()).Returns(Cliente);
            var controle = mapper.Map<ClienteView>(Cliente);
            var retorno = await manager.InsertClienteAsync(NovoCliente);

            await repository.Received().InsertClienteAsync(Arg.Any<Cliente>());
            retorno.Should().BeEquivalentTo(controle);
        }

        [Fact]
        public async Task UpdateClienteAsync_Sucesso()
        {
            repository.UpdateClienteAsync(Arg.Any<Cliente>()).Returns(Cliente);
            var controle = mapper.Map<ClienteView>(Cliente);
            var retorno = await manager.UpdateClienteAsync(AlteraCliente);

            await repository.Received().UpdateClienteAsync(Arg.Any<Cliente>());
            retorno.Should().BeEquivalentTo(controle);
        }

        [Fact]
        public async Task UpdateClienteAsync_NaoEncontrado()
        {
            repository.UpdateClienteAsync(Arg.Any<Cliente>()).ReturnsNull();

            var retorno = await manager.UpdateClienteAsync(AlteraCliente);

            await repository.Received().UpdateClienteAsync(Arg.Any<Cliente>());
            retorno.Should().BeNull();
        }

        [Fact]
        public async Task DeleteClienteAsync_Sucesso()
        {
            repository.DeleteClienteAsync(Arg.Any<int>()).Returns(Cliente);
            var controle = mapper.Map<ClienteView>(Cliente);
            var retorno = await manager.DeleteClienteAsync(Cliente.Id);

            await repository.Received().DeleteClienteAsync(Arg.Any<int>());
            retorno.Should().BeEquivalentTo(controle);
        }

        [Fact]
        public async Task DeleteClienteAsync_NaoEncontrado()
        {
            repository.DeleteClienteAsync(Arg.Any<int>()).ReturnsNull();

            var retorno = await manager.DeleteClienteAsync(1);

            await repository.Received().DeleteClienteAsync(Arg.Any<int>());
            retorno.Should().BeNull();
        }
    }
}