namespace RestauranteAPI.Models
{
    public class Categoria 
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<Plato> Platos { get; set; } = [];
    }
}
