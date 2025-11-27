using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Repositório para gerenciamento de <see cref="ItemPedido"/> no banco de dados.
    /// </summary>
    /// <remarks>
    /// Herda de <see cref="Repository{T}"/> e implementa a interface <see cref="IItemPedidoRepository"/>.
    /// Fornece métodos específicos para adicionar/incrementar itens e obter itens de pedidos.
    /// </remarks>
    public class ItemPedidoRepository : Repository<ItemPedido>, IItemPedidoRepository
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="ItemPedidoRepository"/>.
        /// </summary>
        /// <param name="ctx">Contexto do banco de dados <see cref="CardapioDigitalDbContext"/>.</param>
        public ItemPedidoRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Adiciona um novo item ao pedido ou incrementa a quantidade caso o item já exista.
        /// </summary>
        /// <param name="pedidoId">ID do pedido ao qual o item será adicionado.</param>
        /// <param name="produtoId">ID do produto a ser adicionado ou incrementado.</param>
        /// <param name="quantidade">Quantidade do produto a ser adicionada.</param>
        /// <param name="precoUnitario">Preço unitário do produto.</param>
        /// <returns>Uma tarefa assíncrona representando a operação.</returns>
        public async Task AdicionarOuIncrementarAsync(int pedidoId, int produtoId, int quantidade, decimal precoUnitario)
        {
            var itemExistente = await _dbSet
                .FirstOrDefaultAsync(ip => ip.PedidoId == pedidoId && ip.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                // Incrementa quantidade
                itemExistente.Quantidade += quantidade;

                // Atualiza o preço unitário
                itemExistente.PrecoUnitario = precoUnitario;

                _dbSet.Update(itemExistente);
                return;
            }

            // Se não existe, cria novo item
            var novoItem = new ItemPedido
            {
                PedidoId = pedidoId,
                ProdutoId = produtoId,
                Quantidade = quantidade,
                PrecoUnitario = precoUnitario
            };

            await _dbSet.AddAsync(novoItem);
        }

        /// <summary>
        /// Obtém um item específico de um pedido pelo ID do pedido e do produto.
        /// </summary>
        /// <param name="pedidoId">ID do pedido que contém o item.</param>
        /// <param name="produtoId">ID do produto do item a ser obtido.</param>
        /// <returns>
        /// Uma tarefa assíncrona que retorna o <see cref="ItemPedido"/> se encontrado, ou <c>null</c> caso contrário.
        /// </returns>
        public async Task<ItemPedido?> ObterItemAsync(int pedidoId, int produtoId)
        {
            return await _dbSet.FirstOrDefaultAsync(ip =>
                ip.PedidoId == pedidoId &&
                ip.ProdutoId == produtoId);
        }
    }
}
