using System.ComponentModel.DataAnnotations;

namespace RestauranteAPI.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = "Cliente";

        public DateTime CreadoEn { get; set; }
    }
}