using System.ComponentModel.DataAnnotations;

namespace RestauranteAPI.DTOs
{
    public class AgregarPlatoPedidoDto
    {
        [Required(ErrorMessage = "El plato es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "PlatoId inválido")]
        public int PlatoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 100, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }
    }
}