using System.ComponentModel.DataAnnotations;

namespace RestauranteAPI.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        // Rol por defecto controlado en backend (Cliente por seguridad)
        public string Rol { get; set; } = "Cliente";
    }
}