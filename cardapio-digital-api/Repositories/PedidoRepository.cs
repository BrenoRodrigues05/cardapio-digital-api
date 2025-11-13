using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Implementação do repositório específico para a entidade <see cref="Pedido"/>.
    /// </summary>
    /// <remarks>
    /// Fornece operações especializadas relacionadas à recuperação de pedidos,
    /// incluindo carregamento completo de relacionamentos utilizando Entity Framework.
    /// </remarks>
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="PedidoRepository"/>.
        /// </summary>
        /// <param name="ctx">Contexto do banco de dados utilizado para acesso às entidades.</param>
        public PedidoRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Obtém um pedido completo com todos os seus relacionamentos carregados.
        /// </summary>
        /// <param name="id">Identificador único do pedido a ser recuperado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo o <see cref="Pedido"/> encontrado
        /// (incluindo Cliente, Restaurante, Itens e Produtos dos itens), ou <c>null</c>
        /// caso nenhum pedido correspondente seja localizado.
        /// </returns>
        /// <remarks>
        /// Este método utiliza carregamento explícito (Include/ThenInclude) do Entity Framework
        /// para trazer todas as informações necessárias do pedido em uma única consulta.
        /// </remarks>
        public async Task<Pedido?> GetPedidoCompletoAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Cliente)
                .Include(p => p.Restaurante)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
