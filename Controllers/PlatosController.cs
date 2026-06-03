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
    public class PlatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlatosController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los platos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatoDto>>> GetPlatos()
        {
            var platos = await _context.Platos
                .Select(p => new PlatoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Disponible = p.Disponible,
                    CategoriaId = p.CategoriaId
                })
                .ToListAsync();

            return Ok(platos);
        }

        // Obtener plato por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatoDto>> GetPlato(int id)
        {
            var plato = await _context.Platos
                .Where(p => p.Id == id)
                .Select(p => new PlatoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Disponible = p.Disponible,
                    CategoriaId = p.CategoriaId
                })
                .FirstOrDefaultAsync();

            if (plato == null)
                return NotFound(new { mensaje = "Plato no encontrado." });

            return Ok(plato);
        }

        // Crear plato (solo Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PlatoDto>> PostPlato(PlatoDto dto)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == dto.CategoriaId);

            if (!categoriaExiste)
                return BadRequest(new { mensaje = "La categoría no existe." });

            var platoExiste = await _context.Platos
                .AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower());

            if (platoExiste)
                return BadRequest(new { mensaje = "Ya existe un plato con ese nombre." });

            var plato = new Plato
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Disponible = dto.Disponible,
                CategoriaId = dto.CategoriaId
            };

            _context.Platos.Add(plato);
            await _context.SaveChangesAsync();

            dto.Id = plato.Id;

            return CreatedAtAction(
                nameof(GetPlato),
                new { id = plato.Id },
                dto
            );
        }

        // Actualizar plato (solo Admin)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlato(int id, PlatoDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID del plato." });

            var plato = await _context.Platos.FindAsync(id);

            if (plato == null)
                return NotFound(new { mensaje = "Plato no encontrado." });

            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == dto.CategoriaId);

            if (!categoriaExiste)
                return BadRequest(new { mensaje = "La categoría no existe." });

            var platoExiste = await _context.Platos
                .AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower() && p.Id != id);

            if (platoExiste)
                return BadRequest(new { mensaje = "Ya existe un plato con ese nombre." });

            plato.Nombre = dto.Nombre;
            plato.Descripcion = dto.Descripcion;
            plato.Precio = dto.Precio;
            plato.Disponible = dto.Disponible;
            plato.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Eliminar plato (solo Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlato(int id)
        {
            var plato = await _context.Platos.FindAsync(id);

            if (plato == null)
                return NotFound(new { mensaje = "Plato no encontrado." });

            _context.Platos.Remove(plato);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}