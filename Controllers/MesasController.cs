using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Data;
using RestauranteAPI.DTOs;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MesasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MesasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mesa>>> GetMesas()
        {
            return await _context.Mesas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mesa>> GetMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
                return NotFound();

            return mesa;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Mesa>> PostMesa(MesaDto dto)
        {
            var numeroExiste = await _context.Mesas
                .AnyAsync(m => m.Numero == dto.Numero);

            if (numeroExiste)
                return BadRequest("Ya existe una mesa con ese número.");

            var mesa = new Mesa
            {
                Numero = dto.Numero,
                Capacidad = dto.Capacidad,
                Estado = dto.Estado
            };

            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMesa),
                new { id = mesa.Id },
                mesa
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMesa(int id, MesaDto dto)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
                return NotFound();

            var numeroDuplicado = await _context.Mesas
                .AnyAsync(m => m.Numero == dto.Numero && m.Id != id);

            if (numeroDuplicado)
                return BadRequest("Ya existe una mesa con ese número.");

            mesa.Numero = dto.Numero;
            mesa.Capacidad = dto.Capacidad;
            mesa.Estado = dto.Estado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
                return NotFound();

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}