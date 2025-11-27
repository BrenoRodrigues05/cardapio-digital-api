using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Interface para manipulação de itens de pedido no banco de dados.
    /// </summary>
    /// <remarks>
    /// Herdando de <see cref="IRepository{T}"/>, adiciona operações específicas
    /// relacionadas a <see cref="ItemPedido"/>, como adição/incremento e consulta por pedido e produto.
    /// </remarks>
    public interface IItemPedidoRepository : IRepository<ItemPedido>
    {
        /// <summary>
        /// Adiciona um novo item ao pedido ou incrementa a quantidade caso já exista.
        /// </summary>
        /// <param name="pedidoId">ID do pedido ao qual o item será adicionado.</param>
        /// <param name="produtoId">ID do produto que será adicionado ou incrementado.</param>
        /// <param name="quantidade">Quantidade do produto a ser adicionada.</param>
        /// <param name="precoUnitario">Preço unitário do produto.</param>
        /// <returns>Uma tarefa assíncrona representando a operação.</returns>
        Task AdicionarOuIncrementarAsync(int pedidoId, int produtoId, int quantidade, decimal precoUnitario);

        /// <summary>
        /// Obtém um item específico de um pedido pelo ID do pedido e do produto.
        /// </summary>
        /// <param name="pedidoId">ID do pedido que contém o item.</param>
        /// <param name="produtoId">ID do produto do item a ser obtido.</param>
        /// <returns>
        /// Uma tarefa assíncrona que retorna o <see cref="ItemPedido"/> se encontrado, ou <c>null</c> caso contrário.
        /// </returns>
        Task<ItemPedido?> ObterItemAsync(int pedidoId, int produtoId);
    }
}
