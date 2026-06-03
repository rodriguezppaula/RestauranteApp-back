using System.ComponentModel.DataAnnotations;

namespace RestauranteAPI.DTOs
{
    public class CambiarEstadoPedidoDto
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression(
            "Pendiente|Preparando|Entregado|Cancelado",
            ErrorMessage = "Estado no válido.")]
        public string Estado { get; set; } = string.Empty;
    }
}