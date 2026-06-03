using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Data;
using RestauranteAPI.DTOs;

namespace RestauranteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Rol = u.Rol,
                    CreadoEn = u.CreadoEn
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // Obtener usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Rol = u.Rol,
                    CreadoEn = u.CreadoEn
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(usuario);
        }

        // Actualizar usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el del usuario." });

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == dto.Email && u.Id != id);

            if (emailExiste)
                return BadRequest(new { mensaje = "Ya existe otro usuario con ese email." });

            var rolesValidos = new[] { "Admin", "Mesero", "Cliente" };

            if (!rolesValidos.Contains(dto.Rol))
                return BadRequest(new { mensaje = "Rol inválido." });

            usuario.Nombre = dto.Nombre;
            usuario.Email = dto.Email;
            usuario.Rol = dto.Rol;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Eliminar usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}