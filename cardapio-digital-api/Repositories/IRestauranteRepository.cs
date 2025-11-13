using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Repositório específico para operações relacionadas à entidade <see cref="Restaurante"/>.
    /// </summary>
    /// <remarks>
    /// Esta interface estende <see cref="IRepository{Restaurante}"/> e define métodos
    /// especializados para recuperar dados completos do restaurante, incluindo seus
    /// relacionamentos.
    /// </remarks>
    public interface IRestauranteRepository : IRepository<Restaurante>
    {
        /// <summary>
        /// Obtém um restaurante com todos os seus relacionamentos carregados.
        /// </summary>
        /// <param name="id">O identificador único do restaurante a ser recuperado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> que, ao completar, contém um objeto
        /// <see cref="Restaurante"/> com todos os dados relacionados (como horários de
        /// funcionamento, endereço, categorias, produtos, configurações, etc.), ou
        /// <c>null</c> se nenhum restaurante com o ID fornecido for encontrado.
        /// </returns>
        /// <remarks>
        /// Esse método deve ser utilizado quando é necessário acessar o restaurante
        /// com todas as suas entidades relacionadas, evitando múltiplas consultas
        /// adicionais. A implementação concreta geralmente utiliza carregamento
        /// explícito ou Includes no Entity Framework.
        /// </remarks>
        Task<Restaurante?> GetRestauranteCompletoAsync(int id);
    }
}
