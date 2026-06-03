using System.ComponentModel.DataAnnotations;

namespace RestauranteAPI.DTOs
{
    public class PlatoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del plato es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0.01, 999999, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; } = true;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int CategoriaId { get; set; }
    }
}