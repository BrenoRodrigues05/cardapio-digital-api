using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Implementação do repositório específico para a entidade <see cref="Restaurante"/>.
    /// </summary>
    /// <remarks>
    /// Fornece métodos especializados para recuperar restaurantes e carregar
    /// seus relacionamentos essenciais, como produtos e pedidos associados.
    /// </remarks>
    public class RestauranteRepository : Repository<Restaurante>, IRestauranteRepository
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="RestauranteRepository"/>.
        /// </summary>
        /// <param name="ctx">Contexto do banco de dados utilizado para acesso às entidades.</param>
        public RestauranteRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Obtém um restaurante com todos os seus relacionamentos carregados.
        /// </summary>
        /// <param name="id">Identificador único do restaurante desejado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo o <see cref="Restaurante"/> encontrado
        /// (com Produtos e Pedidos carregados), ou <c>null</c> caso não seja localizado.
        /// </returns>
        /// <remarks>
        /// Esse método utiliza carregamento explícito via <c>Include</c> para trazer,
        /// em uma única consulta, os produtos e pedidos associados ao restaurante.
        /// Ideal para exibições completas ou operações que dependem de seus relacionamentos.
        /// </remarks>
        public async Task<Restaurante?> GetRestauranteCompletoAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Produtos)
                .Include(r => r.Pedidos)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
