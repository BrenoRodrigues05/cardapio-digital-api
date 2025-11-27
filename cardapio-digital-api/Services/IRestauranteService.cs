using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    public interface IRestauranteService
    {
        Task<IEnumerable<Restaurante>> ObterTodosAsync();
        Task<Restaurante?> ObterPorIdAsync(int id);
        Task<Restaurante> CriarAsync(Restaurante restaurante);
        Task<bool> AtualizarAsync(Restaurante restaurante);
        Task<bool> RemoverAsync(int id);
        Task<IEnumerable<Restaurante>> BuscarPorNomeAsync(string nome);
        Task<IEnumerable<Restaurante>> BuscarPorEnderecoAsync(string endereco);
        Task<bool> RestauranteExisteAsync(int id);
        Task<bool> RestauranteComNomeExisteAsync(string nome, int? restauranteId = null);
        Task<int> ObterTotalPedidosAsync(int restauranteId);
        Task<int> ObterTotalProdutosAsync(int restauranteId);
    }
}
