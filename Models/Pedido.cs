namespace RestauranteAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int MesaId { get; set; }
        public Mesa Mesa { get; set; } = null!;
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public string Estado { get; set; } = "Pendiente";
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public ICollection<PedidoPlato> PedidoPlatos { get; set; } = [];
    }

    public class PedidoPlato
    {
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public int PlatoId { get; set; }
        public Plato Plato { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}
