using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Interface que define os serviços relacionados a itens de pedido.
    /// </summary>
    /// <remarks>
    /// Fornece operações CRUD básicas e funcionalidades específicas
    /// como cálculo de subtotal, validação de estoque e adição/incremento de itens.
    /// </remarks>
    public interface IItemPedidoService
    {
        #region CRUD Básico

        /// <summary>
        /// Obtém um item de pedido pelo seu ID.
        /// </summary>
        /// <param name="id">ID do item de pedido.</param>
        /// <returns>O <see cref="ItemPedido"/> correspondente ou <c>null</c> se não encontrado.</returns>
        Task<ItemPedido?> ObterPorIdAsync(int id);

        /// <summary>
        /// Cria um novo item de pedido.
        /// </summary>
        /// <param name="itemPedido">Objeto <see cref="ItemPedido"/> a ser criado.</param>
        /// <returns>O <see cref="ItemPedido"/> criado com o ID gerado.</returns>
        Task<ItemPedido> CriarAsync(ItemPedido itemPedido);

        /// <summary>
        /// Atualiza os dados de um item de pedido existente.
        /// </summary>
        /// <param name="itemPedido">Objeto <see cref="ItemPedido"/> com dados atualizados.</param>
        /// <returns><c>true</c> se a atualização foi realizada com sucesso; caso contrário, <c>false</c>.</returns>
        Task<bool> AtualizarAsync(ItemPedido itemPedido);

        /// <summary>
        /// Remove um item de pedido pelo seu ID.
        /// </summary>
        /// <param name="id">ID do item de pedido a ser removido.</param>
        /// <returns><c>true</c> se o item foi removido com sucesso; caso contrário, <c>false</c>.</returns>
        Task<bool> RemoverAsync(int id);

        #endregion

        #region Operações Principais

        /// <summary>
        /// Obtém todos os itens de um pedido específico.
        /// </summary>
        /// <param name="pedidoId">ID do pedido.</param>
        /// <returns>Uma coleção de <see cref="ItemPedido"/> do pedido informado.</returns>
        Task<IEnumerable<ItemPedido>> ObterItensPorPedidoAsync(int pedidoId);

        /// <summary>
        /// Calcula o subtotal de um pedido, somando os preços dos itens multiplicados pelas quantidades.
        /// </summary>
        /// <param name="pedidoId">ID do pedido.</param>
        /// <returns>O subtotal do pedido como <see cref="decimal"/>.</returns>
        Task<decimal> CalcularSubtotalDoPedidoAsync(int pedidoId);

        /// <summary>
        /// Valida se a quantidade solicitada de um produto está disponível em estoque.
        /// </summary>
        /// <param name="produtoId">ID do produto.</param>
        /// <param name="quantidade">Quantidade solicitada.</param>
        /// <returns><c>true</c> se houver estoque suficiente; caso contrário, <c>false</c>.</returns>
        Task<bool> ValidarDisponibilidadeEEstoqueAsync(int produtoId, int quantidade);

        /// <summary>
        /// Adiciona um item ao pedido ou incrementa a quantidade se o item já existir.
        /// </summary>
        /// <param name="pedidoId">ID do pedido.</param>
        /// <param name="produtoId">ID do produto a ser adicionado ou incrementado.</param>
        /// <param name="quantidade">Quantidade a ser adicionada.</param>
        /// <param name="precoUnitario">Preço unitário do produto.</param>
        /// <returns>O <see cref="ItemPedido"/> atualizado ou recém-criado.</returns>
        Task<ItemPedido> AdicionarOuIncrementarAsync(int pedidoId, int produtoId, int quantidade, decimal precoUnitario);

        #endregion
    }
}
