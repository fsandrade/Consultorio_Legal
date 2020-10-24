using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CL.Core.Shared.ModelViews.Erro;
using CL.Core.Shared.ModelViews.Medico;
using CL.Manager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CL.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoManager manager;

        public MedicosController(IMedicoManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Retorna todos os médicos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MedicoView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await manager.GetMedicosAsync());
        }

        /// <summary>
        /// Retorna um médico consultado via ID
        /// </summary>
        /// <param name="id" example="123">Id do médico</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MedicoView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await manager.GetMedicoAsync(id)); ;
        }

        /// <summary>
        /// Insere um novo médico.
        /// </summary>
        /// <param name="medico"></param>
        [HttpPost]
        [ProducesResponseType(typeof(MedicoView), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(NovoMedico medico)
        {
            var medicoInserido = await manager.InsertMedicoAsync(medico);
            return CreatedAtAction(nameof(Get), new { id = medicoInserido.Id }, medicoInserido);
        }

        /// <summary>
        /// Altera um médico
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(MedicoView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(AlteraMedico medico)
        {
            var medicoAtualizado = await manager.UpdateMedicoAsync(medico);
            if (medicoAtualizado == null)
            {
                return NotFound();
            }
            return Ok(medicoAtualizado);
        }

        /// <summary>
        /// Exclui um médico.
        /// </summary>
        /// <param name="id" example="123">Id do médico</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await manager.DeleteMedicoAsync(id);
            return NoContent();
        }
    }
}