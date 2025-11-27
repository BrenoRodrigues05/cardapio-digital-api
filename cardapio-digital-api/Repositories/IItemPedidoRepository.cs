using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    public interface IItemPedidoRepository : IRepository<ItemPedido>
    {
        Task AdicionarOuIncrementarAsync(int pedidoId, int produtoId, int quantidade, decimal precoUnitario);
        Task<ItemPedido?> ObterItemAsync(int pedidoId, int produtoId);
    }
}
