using CL.Core.Shared.ModelViews.Cliente;
using CL.FakeData.ClienteData;
using CL.Manager.Interfaces.Managers;
using CL.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CL.WebApi.Tests.Crontrollers
{
    public class ClientesControllerTest
    {
        private readonly IClienteManager manger;
        private readonly ILogger<ClientesController> logger;
        private readonly ClientesController controller;
        private readonly ClienteView clienteView;
        private readonly List<ClienteView> listaClienteView;

        public ClientesControllerTest()
        {
            manger = Substitute.For<IClienteManager>();
            logger = Substitute.For<ILogger<ClientesController>>();
            controller = new ClientesController(manger, logger);

            clienteView = new ClienteViewFaker().Generate();
            listaClienteView = new ClienteViewFaker().Generate(10);
        }

        [Fact]
        public async Task Get_Ok()
        {
            var controle = new List<ClienteView>();
            listaClienteView.ForEach(p => controle.Add(p.CloneTipado()));

            manger.GetClientesAsync().Returns(listaClienteView);
            var resultado = (ObjectResult)await controller.Get();
            resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
            resultado.Value.Should().BeEquivalentTo(controle);
        }
    }
}