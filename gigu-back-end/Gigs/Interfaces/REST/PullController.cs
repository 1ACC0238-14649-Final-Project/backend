using Gigs.Domain.Services;
using Gigs.Domain.Models.Entities;
using Gigs.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigs.Domain.Services.CommandServices;
using Gigs.Domain.Services.QueryServices;

namespace Gigs.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class PullController : ControllerBase
    {
        private readonly IPullQueryService _pullQuery;
        private readonly IPullCommandService _pullCommand;

        public PullController(IPullQueryService pullQuery, IPullCommandService pullCommand)
        {
            _pullQuery = pullQuery;
            _pullCommand = pullCommand;
        }

        /// <summary>
        /// Obtiene todos los Pulls disponibles.
        /// </summary>
        /// <returns>Lista de Pulls.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PullResource>), 200)]
        public async Task<ActionResult<IEnumerable<PullResource>>> GetAll()
        {
            var pulls = await _pullQuery.GetAllPullsAsync();
            var resources = pulls.Select(p => new PullResource
            {
                Id = p.Id,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId,
                GigId = p.GigId,
                PriceInit = p.PriceInit,
                PriceUpdate = p.PriceUpdate,
                State = p.State
            });
            return Ok(resources);
        }

        /// <summary>
        /// Obtiene un Pull por su ID.
        /// </summary>
        /// <param name="id">ID del Pull</param>
        /// <returns>Un Pull si existe.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PullResource), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PullResource>> GetById(int id)
        {
            var pull = await _pullQuery.GetPullByIdAsync(id);
            if (pull == null) return NotFound();

            return Ok(new PullResource
            {
                Id = pull.Id,
                SellerId = pull.SellerId,
                BuyerId = pull.BuyerId,
                GigId = pull.GigId,
                PriceInit = pull.PriceInit,
                PriceUpdate = pull.PriceUpdate,
                State = pull.State
            });
        }

        /// <summary>
        /// Obtiene Pulls filtrados por rol (seller o buyer) e ID de usuario.
        /// </summary>
        /// <param name="role">Rol: 'seller' o 'buyer'</param>
        /// <param name="userId">ID del usuario correspondiente</param>
        /// <returns>Lista de Pulls según el filtro.</returns>
        [HttpGet("by-role")]
        [ProducesResponseType(typeof(IEnumerable<PullResource>), 200)]
        public async Task<ActionResult<IEnumerable<PullResource>>> GetByRole([FromQuery] string role, [FromQuery] int userId)
        {
            var pulls = await _pullQuery.GetPullsByRoleAsync(role, userId);
            var result = pulls.Select(p => new PullResource
            {
                Id = p.Id,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId,
                GigId = p.GigId,
                PriceInit = p.PriceInit,
                PriceUpdate = p.PriceUpdate,
                State = p.State
            });
            return Ok(result);
        }

        /// <summary>
        /// Crea un nuevo Pull.
        /// </summary>
        /// <param name="resource">Datos para crear el Pull</param>
        /// <returns>Pull creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PullResource), 201)]
        public async Task<ActionResult<PullResource>> Create([FromBody] CreatePullResource resource)
        {
            var pull = new Pull(resource.SellerId, resource.GigId, resource.PriceInit)
            {
                BuyerId = resource.BuyerId,
                State = resource.State ?? "pending"
            };
            await _pullCommand.OpenPullAsync(pull);
            return CreatedAtAction(nameof(GetById), new { id = pull.Id }, new PullResource
            {
                Id = pull.Id,
                SellerId = pull.SellerId,
                BuyerId = pull.BuyerId,
                GigId = pull.GigId,
                PriceInit = pull.PriceInit,
                PriceUpdate = pull.PriceUpdate,
                State = pull.State
            });
        }

        /// <summary>
        /// Actualiza el precio o estado de un Pull.
        /// </summary>
        /// <param name="id">ID del Pull</param>
        /// <param name="request">Nuevos valores</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PullResource), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePullRequest request)
        {
            try
            {
                var updated = await _pullCommand.UpdatePullAsync(id, request.NewPrice, request.NewState);
                return Ok(new PullResource
                {
                    Id = updated.Id,
                    SellerId = updated.SellerId,
                    BuyerId = updated.BuyerId,
                    GigId = updated.GigId,
                    PriceInit = updated.PriceInit,
                    PriceUpdate = updated.PriceUpdate,
                    State = updated.State
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cierra un Pull cambiando su estado a 'complete'.
        /// </summary>
        /// <param name="id">ID del Pull</param>
        [HttpPut("{id}/close")]
        [ProducesResponseType(typeof(PullResource), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Close(int id)
        {
            try
            {
                var closed = await _pullCommand.ClosePullAsync(id);
                return Ok(closed);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Payload para actualizar Pull.
        /// </summary>
        public class UpdatePullRequest
        {
            public decimal? NewPrice { get; set; }
            public string? NewState { get; set; }
        }
    }
}
