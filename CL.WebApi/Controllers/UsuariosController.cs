using CL.Core.Domain;
using CL.Core.Shared.ModelViews.Usuario;
using CL.Manager.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
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
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            var usuarioLogado = await manager.ValidaUsuarioEGeraTokenAsync(usuario);
            if (usuarioLogado != null)
            {
                return Ok(usuarioLogado);
            }
            return Unauthorized();
        }

        [Authorize(Roles = "Presidente, Lider")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string login = User.Identity.Name;
            var usuario = await manager.GetAsync(login);
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Post(NovoUsuario usuario)
        {
            var usuarioInserido = await manager.InsertAsync(usuario);
            return CreatedAtAction(nameof(Get), new { login = usuario.Login }, usuarioInserido);
        }
    }
}