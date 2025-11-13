using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    public interface IRestauranteRepository : IRepository<Restaurante>
    {
        Task<Restaurante?> GetRestauranteCompletoAsync(int id);
    }
}
