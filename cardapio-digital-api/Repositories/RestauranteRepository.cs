using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    public class RestauranteRepository : Repository<Restaurante>, IRestauranteRepository
    {
        public RestauranteRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

        public async Task<Restaurante?> GetRestauranteCompletoAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Produtos)
                .Include(r => r.Pedidos)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
