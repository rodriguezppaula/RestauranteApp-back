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
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las categorías
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // Obtener categoría por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Where(c => c.Id == id)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                })
                .FirstOrDefaultAsync();

            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            return Ok(categoria);
        }

        // Crear categoría (solo Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> PostCategoria(CategoriaDto dto)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower());

            if (categoriaExiste)
                return BadRequest(new { mensaje = "Ya existe una categoría con ese nombre." });

            var categoria = new Categoria
            {
                Nombre = dto.Nombre
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            dto.Id = categoria.Id;

            return CreatedAtAction(
                nameof(GetCategoria),
                new { id = categoria.Id },
                dto
            );
        }

        // Actualizar categoría (solo Admin)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, CategoriaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID de la categoría." });

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower() && c.Id != id);

            if (categoriaExiste)
                return BadRequest(new { mensaje = "Ya existe una categoría con ese nombre." });

            categoria.Nombre = dto.Nombre;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Eliminar categoría (solo Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            _context.Categorias.Remove(categoria);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}