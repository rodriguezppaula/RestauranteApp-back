using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RestauranteAPI.Data;
using RestauranteAPI.DTOs;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.PedidoPlatos)
                    .ThenInclude(pp => pp.Plato)
                .ToListAsync();
        }

        // Obtener pedido por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.PedidoPlatos)
                    .ThenInclude(pp => pp.Plato)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado." });

            return Ok(pedido);
        }

        // Crear pedido (Admin o Mesero)
        [Authorize(Roles = "Admin,Mesero")]
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(CrearPedidoDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var usuarioId = int.Parse(userIdClaim);

            var mesa = await _context.Mesas.FindAsync(dto.MesaId);

            if (mesa == null)
                return BadRequest(new { mensaje = "La mesa no existe." });

            if (mesa.Estado == "Ocupada")
                return BadRequest(new { mensaje = "La mesa ya se encuentra ocupada." });

            mesa.Estado = "Ocupada";

            var pedido = new Pedido
            {
                MesaId = dto.MesaId,
                UsuarioId = usuarioId,
                Estado = "Pendiente"
            };

            _context.Pedidos.Add(pedido);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPedido),
                new { id = pedido.Id },
                pedido
            );
        }

        // Agregar plato a un pedido
        [Authorize(Roles = "Admin,Mesero")]
        [HttpPost("{id}/platos")]
        public async Task<IActionResult> AgregarPlato(
            int id,
            AgregarPlatoPedidoDto dto)
        {
            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado." });

            var plato = await _context.Platos
                .FirstOrDefaultAsync(p => p.Id == dto.PlatoId);

            if (plato == null)
                return BadRequest(new { mensaje = "El plato no existe." });

            if (!plato.Disponible)
                return BadRequest(new { mensaje = "El plato no está disponible." });

            var pedidoPlatoExistente = await _context.PedidoPlatos
                .FirstOrDefaultAsync(pp =>
                    pp.PedidoId == id &&
                    pp.PlatoId == dto.PlatoId);

            if (pedidoPlatoExistente != null)
            {
                pedidoPlatoExistente.Cantidad += dto.Cantidad;
            }
            else
            {
                var pedidoPlato = new PedidoPlato
                {
                    PedidoId = id,
                    PlatoId = dto.PlatoId,
                    Cantidad = dto.Cantidad
                };

                _context.PedidoPlatos.Add(pedidoPlato);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Plato agregado correctamente al pedido."
            });
        }

        // Cambiar estado del pedido
        [Authorize(Roles = "Admin,Mesero")]
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoPedido(
            int id,
            CambiarEstadoPedidoDto dto)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Mesa)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado." });

            pedido.Estado = dto.Estado;

            if (dto.Estado == "Entregado" || dto.Estado == "Cancelado")
            {
                pedido.Mesa.Estado = "Disponible";
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Estado actualizado correctamente.",
                pedido.Id,
                pedido.Estado
            });
        }

        // Eliminar pedido (solo Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado." });

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}