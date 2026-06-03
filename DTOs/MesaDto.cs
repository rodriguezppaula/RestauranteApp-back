using System.ComponentModel.DataAnnotations;

namespace RestauranteAPI.DTOs
{
    public class MesaDto
    {
        [Required(ErrorMessage = "El número de mesa es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de mesa debe ser mayor a 0")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20 personas")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [RegularExpression("Disponible|Ocupada",
            ErrorMessage = "El estado solo puede ser Disponible u Ocupada")]
        public string Estado { get; set; } = "Disponible";
    }
}