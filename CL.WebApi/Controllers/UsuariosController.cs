using CL.Core.Domain;
using CL.Manager.Interfaces.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CL.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioManager manager;

        public UsuariosController(IUsuarioManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        [Route("ValidaUsuario")]
        public async Task<IActionResult> ValidaUsuario([FromBody] Usuario usuario)
        {
            var valido = await manager.ValidaSenhaAsync(usuario);
            if (valido)
            {
                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet("{login}")]
        public string Get(string login)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> Post(Usuario usuario)
        {
            var usuarioInserido = await manager.InsertAsync(usuario);
            return CreatedAtAction(nameof(Get), new { login = usuario.Login }, usuarioInserido);
        }
    }
}