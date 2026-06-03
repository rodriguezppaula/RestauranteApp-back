using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Models;

namespace RestauranteAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Plato> Platos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoPlato> PedidoPlatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoPlato>()
                .HasKey(pp => new { pp.PedidoId, pp.PlatoId });

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Plato>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Entradas" },
                new Categoria { Id = 2, Nombre = "Platos fuertes" },
                new Categoria { Id = 3, Nombre = "Bebidas" },
                new Categoria { Id = 4, Nombre = "Postres" }
            );

            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Email = "admin@restaurante.com",
                PasswordHash = "$2a$11$pzFBPEPXNHBKgtT5DCAU7.8GtQhBdnJJkBrLYMEUjW6cnJ1FpKuqu",
                Rol = "Admin",
                CreadoEn = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }
    }
}