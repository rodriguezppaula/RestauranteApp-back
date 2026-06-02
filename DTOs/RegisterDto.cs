using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace RestauranteAPI.DTOs
{
    public class RegisterDto
    {
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string Rol { get; set; } = "Cliente";
    }
}
