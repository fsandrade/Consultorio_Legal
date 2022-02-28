using CL.Core.Shared.ModelViews.Cliente;
using CL.FakeData.ClienteData;

namespace CL.WebApi.Tests.Crontrollers;

public class ClientesControllerTest
{
    private readonly IClienteManager manager;
    private readonly ILogger<ClientesController> logger;
    private readonly ClientesController controller;
    private readonly ClienteView clienteView;
    private readonly List<ClienteView> listaClienteView;
    private readonly NovoCliente novoCliente;

    public ClientesControllerTest()
    {
        manager = Substitute.For<IClienteManager>();
        logger = Substitute.For<ILogger<ClientesController>>();
        controller = new ClientesController(manager, logger);

        clienteView = new ClienteViewFaker().Generate();
        listaClienteView = new ClienteViewFaker().Generate(10);
        novoCliente = new NovoClienteFaker().Generate();
    }

    [Fact]
    public async Task Get_Ok()
    {
        var controle = new List<ClienteView>();
        listaClienteView.ForEach(p => controle.Add(p.CloneTipado()));
        manager.GetClientesAsync().Returns(listaClienteView);

        var resultado = (ObjectResult)await controller.Get();

        await manager.Received().GetClientesAsync();
        resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
        resultado.Value.Should().BeEquivalentTo(controle);
    }

    [Fact]
    public async Task Get_NotFound()
    {
        manager.GetClientesAsync().Returns(new List<ClienteView>());

        var resultado = (StatusCodeResult)await controller.Get();

        await manager.Received().GetClientesAsync();
        resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task GetById_Ok()
    {
        manager.GetClienteAsync(Arg.Any<int>()).Returns(clienteView.CloneTipado());

        var resultado = (ObjectResult)await controller.Get(clienteView.Id);

        await manager.Received().GetClienteAsync(Arg.Any<int>());
        resultado.Value.Should().BeEquivalentTo(clienteView);
        resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetById_NotFound()
    {
        manager.GetClienteAsync(Arg.Any<int>()).Returns(new ClienteView());

        var resultado = (StatusCodeResult)await controller.Get(1);

        await manager.Received().GetClienteAsync(Arg.Any<int>());
        resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Post_Created()
    {
        manager.InsertClienteAsync(Arg.Any<NovoCliente>()).Returns(clienteView.CloneTipado());

        var resultado = (ObjectResult)await controller.Post(novoCliente);

        await manager.Received().InsertClienteAsync(Arg.Any<NovoCliente>());
        resultado.StatusCode.Should().Be(StatusCodes.Status201Created);
        resultado.Value.Should().BeEquivalentTo(clienteView);
    }

    [Fact]
    public async Task Put_Ok()
    {
        manager.UpdateClienteAsync(Arg.Any<AlteraCliente>()).Returns(clienteView.CloneTipado());

        var resultado = (ObjectResult)await controller.Put(new AlteraCliente());

        await manager.Received().UpdateClienteAsync(Arg.Any<AlteraCliente>());
        resultado.StatusCode.Should().Be(StatusCodes.Status200OK);
        resultado.Value.Should().BeEquivalentTo(clienteView);
    }

    [Fact]
    public async Task Put_NotFound()
    {
        manager.UpdateClienteAsync(Arg.Any<AlteraCliente>()).ReturnsNull();

        var resultado = (StatusCodeResult)await controller.Put(new AlteraCliente());

        await manager.Received().UpdateClienteAsync(Arg.Any<AlteraCliente>());
        resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Delete_NoContent()
    {
        manager.DeleteClienteAsync(Arg.Any<int>()).Returns(clienteView);

        var resultado = (StatusCodeResult)await controller.Delete(1);

        await manager.Received().DeleteClienteAsync(Arg.Any<int>());
        resultado.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task NotFound_NotFound()
    {
        manager.DeleteClienteAsync(Arg.Any<int>()).ReturnsNull();

        var resultado = (StatusCodeResult)await controller.Delete(1);

        await manager.Received().DeleteClienteAsync(Arg.Any<int>());
        resultado.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}