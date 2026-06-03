using System.Text.Json.Serialization;

namespace RestauranteAPI.Models
{
    public class Plato
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public bool Disponible { get; set; } = true;

        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }
    }
}