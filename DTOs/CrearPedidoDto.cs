using System.ComponentModel.DataAnnotations;

namespace RestauranteAPI.DTOs
{
    public class CrearPedidoDto
    {
        [Required(ErrorMessage = "La mesa es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "MesaId inválido")]
        public int MesaId { get; set; }
    }
}