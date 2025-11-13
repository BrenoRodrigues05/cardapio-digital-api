using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosDisponiveisAsync();
        Task<Produto?> GetProdutoCompletoAsync(int id);
    }
}
