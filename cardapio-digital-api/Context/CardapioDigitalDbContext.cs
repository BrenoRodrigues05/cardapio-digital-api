using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Context
{
    /// <summary>
    /// Representa o contexto do banco de dados para o sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Gerencia as entidades <see cref="Cliente"/>, <see cref="Restaurante"/>,
    /// <see cref="Produto"/>, <see cref="Pedido"/> e <see cref="ItemPedido"/>.
    /// Configura os relacionamentos entre restaurantes, produtos, pedidos e itens de pedidos.
    /// </remarks>
    public class CardapioDigitalDbContext : DbContext
    {
        /// <summary>
        /// Construtor do DbContext, recebendo opções de configuração.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext.</param>
        public CardapioDigitalDbContext(DbContextOptions<CardapioDigitalDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet de clientes cadastrados.
        /// </summary>
        public DbSet<Cliente> Clientes { get; set; }

        /// <summary>
        /// DbSet de restaurantes cadastrados.
        /// </summary>
        public DbSet<Restaurante> Restaurantes { get; set; }

        /// <summary>
        /// DbSet de produtos disponíveis nos restaurantes.
        /// </summary>
        public DbSet<Produto> Produtos { get; set; }

        /// <summary>
        /// DbSet de pedidos realizados pelos clientes.
        /// </summary>
        public DbSet<Pedido> Pedidos { get; set; }

        /// <summary>
        /// DbSet de itens que compõem os pedidos.
        /// </summary>
        public DbSet<ItemPedido> ItensPedido { get; set; }

        /// <summary>
        /// Configura os relacionamentos entre entidades usando Fluent API.
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelo do Entity Framework Core.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Restaurante → Produto (1:N)
            modelBuilder.Entity<Restaurante>()
                .HasMany(r => r.Produtos)
                .WithOne(p => p.Restaurante)
                .HasForeignKey(p => p.RestauranteId);

            // Pedido → ItemPedido (1:N)
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId);

            // Produto → ItemPedido (1:N)
            modelBuilder.Entity<Produto>()
                .HasMany(p => p.ItensPedido)
                .WithOne(i => i.Produto)
                .HasForeignKey(i => i.ProdutoId);
        }
    }
}
