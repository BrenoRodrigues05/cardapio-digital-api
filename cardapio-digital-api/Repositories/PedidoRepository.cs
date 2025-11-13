using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

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
