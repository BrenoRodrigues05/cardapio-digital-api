using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }
        public async Task<IEnumerable<Produto>> GetProdutosDisponiveisAsync()
        {
           return await _dbSet.Where(p => p.Disponivel).ToListAsync();
        }
        public async Task<Produto?> GetProdutoCompletoAsync(int id)
        {
           return await _dbSet
                .Include(p => p.Restaurante)
                .Include(p => p.ItensPedido)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

       
    }
}
