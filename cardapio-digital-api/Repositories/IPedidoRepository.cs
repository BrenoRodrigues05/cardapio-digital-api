using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido?> GetPedidoCompletoAsync(int id);
    }
}
