using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Data;
using RestauranteAPI.DTOs;
using RestauranteAPI.Models;
using RestauranteAPI.Services;


namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(AppDbContext db, TokenService tokenService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await db.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { mensaje = "El email ya está registrado." });

            var rolesValidos = new[] { "Admin", "Mesero", "Cliente" };
            if (!rolesValidos.Contains(dto.Rol))
                return BadRequest(new { mensaje = "Rol inválido." });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Rol = dto.Rol
            };

            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();

            var token = tokenService.GenerarToken(usuario);
            return Ok(new { token, usuario.Email, usuario.Rol, usuario.Nombre });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var usuario = await db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            var token = tokenService.GenerarToken(usuario);
            return Ok(new { token, usuario.Email, usuario.Rol, usuario.Nombre });
        }
    }
}
