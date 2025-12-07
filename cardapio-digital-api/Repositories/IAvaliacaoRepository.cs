using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Define a interface para operações de acesso a dados da entidade <see cref="Avaliacao"/>.
    /// Inclui consultas específicas relacionadas a clientes, restaurantes e entregadores.
    /// </summary>
    public interface IAvaliacaoRepository : IRepository<Avaliacao>
    {
        /// <summary>
        /// Obtém todas as avaliações associadas a um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>Uma coleção de avaliações referentes ao restaurante informado.</returns>
        Task<IEnumerable<Avaliacao>> GetByRestauranteAsync(int restauranteId);

        /// <summary>
        /// Obtém todas as avaliações realizadas por um cliente específico.
        /// </summary>
        /// <param name="clienteId">ID do cliente.</param>
        /// <returns>Uma coleção de avaliações feitas pelo cliente informado.</returns>
        Task<IEnumerable<Avaliacao>> GetByClienteAsync(int clienteId);

        /// <summary>
        /// Calcula a média das notas atribuídas a um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>
        /// A média das notas recebidas pelo restaurante, ou 0 caso não existam avaliações.
        /// </returns>
        Task<double> GetMediaNotasRestauranteAsync(int restauranteId);

        /// <summary>
        /// Obtém todas as avaliações destinadas a um entregador específico.
        /// </summary>
        /// <param name="entregadorId">ID do entregador.</param>
        /// <returns>Uma coleção de avaliações relacionadas ao entregador informado.</returns>
        Task<IEnumerable<Avaliacao>> GetByEntregadorAsync(int entregadorId);

        /// <summary>
        /// Calcula a média das notas recebidas por um entregador específico.
        /// </summary>
        /// <param name="entregadorId">ID do entregador.</param>
        /// <returns>
        /// A média das notas do entregador, ou 0 caso ele ainda não tenha avaliações.
        /// </returns>
        Task<double> GetMediaNotasEntregadorAsync(int entregadorId);
    }
}
