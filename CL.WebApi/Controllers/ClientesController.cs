using CL.Core.Domain;
using CL.Core.Shared.ModelViews.Cliente;
using CL.Manager.Interfaces.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SerilogTimings;
using System.Linq;
using System.Threading.Tasks;

namespace CL.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteManager clienteManager;
        private readonly ILogger<ClientesController> logger;

        public ClientesController(IClienteManager clienteManager, ILogger<ClientesController> logger)
        {
            this.clienteManager = clienteManager;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todos clientes cadastrados na base.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ClienteView), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var clientes = await clienteManager.GetClientesAsync();
            if (clientes.Any())
            {
                return Ok(clientes);
            }
            return NotFound();
        }

        /// <summary>
        /// Retorna um cliente consultado pelo id.
        /// </summary>
        /// <param name="id" example="123">Id do cliente.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteView), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await clienteManager.GetClienteAsync(id);
            if (cliente.Id == 0)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        /// <summary>
        /// Insere um novo cliente
        /// </summary>
        /// <param name="novoCliente"></param>
        [HttpPost]
        [ProducesResponseType(typeof(ClienteView), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(NovoCliente novoCliente)
        {
            logger.LogInformation("Objeto recebido {@novoCliente}", novoCliente);

            ClienteView clienteInserido;
            using (Operation.Time("Tempo de adição de um novo cliente."))
            {
                logger.LogInformation("Foi requisitada a inserção de um novo cliente.");
                clienteInserido = await clienteManager.InsertClienteAsync(novoCliente);
            }
            return CreatedAtAction(nameof(Get), new { id = clienteInserido.Id }, clienteInserido);
        }

        /// <summary>
        /// Altera um cliente.
        /// </summary>
        /// <param name="alteraCliente"></param>
        [HttpPut]
        [ProducesResponseType(typeof(ClienteView), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(AlteraCliente alteraCliente)
        {
            var clienteAtualizado = await clienteManager.UpdateClienteAsync(alteraCliente);
            if (clienteAtualizado == null)
            {
                return NotFound();
            }
            return Ok(clienteAtualizado);
        }

        /// <summary>
        /// Exclui um cliente.
        /// </summary>
        /// <param name="id" example="123">Id do cliente</param>
        /// <remarks>Ao excluir um cliente o mesmo será removido permanentemente da base.</remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var clienteExcliudo = await clienteManager.DeleteClienteAsync(id);
            if (clienteExcliudo == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}