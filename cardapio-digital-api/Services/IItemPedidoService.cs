using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    public interface IItemPedidoService
    {
        // CRUD Básico
        Task<ItemPedido?> ObterPorIdAsync(int id);
        Task<ItemPedido> CriarAsync(ItemPedido itemPedido);
        Task<bool> AtualizarAsync(ItemPedido itemPedido);
        Task<bool> RemoverAsync(int id);

        // Operações Principais
        Task<IEnumerable<ItemPedido>> ObterItensPorPedidoAsync(int pedidoId);
        Task<decimal> CalcularSubtotalDoPedidoAsync(int pedidoId);
        Task<bool> ValidarDisponibilidadeEEstoqueAsync(int produtoId, int quantidade);
        Task<ItemPedido> AdicionarOuIncrementarAsync(int pedidoId, int produtoId, int quantidade, decimal precoUnitario);

    }
}
