using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Interface que define os serviços relacionados a clientes.
    /// </summary>
    /// <remarks>
    /// Fornece operações para criar, atualizar, remover e consultar clientes no sistema.
    /// </remarks>
    public interface IClienteService
    {
        /// <summary>
        /// Cria um novo cliente no sistema.
        /// </summary>
        /// <param name="cliente">Objeto <see cref="Cliente"/> a ser criado.</param>
        /// <returns>O <see cref="Cliente"/> criado com o ID gerado.</returns>
        Task<Cliente> CriarAsync(Cliente cliente);

        /// <summary>
        /// Obtém um cliente pelo seu ID.
        /// </summary>
        /// <param name="id">ID do cliente a ser obtido.</param>
        /// <returns>O <see cref="Cliente"/> correspondente ou <c>null</c> se não encontrado.</returns>
        Task<Cliente?> ObterPorIdAsync(int id);

        /// <summary>
        /// Obtém todos os clientes cadastrados.
        /// </summary>
        /// <returns>Uma coleção de <see cref="Cliente"/>.</returns>
        Task<IEnumerable<Cliente>> ObterTodosAsync();

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="cliente">Objeto <see cref="Cliente"/> com dados atualizados.</param>
        /// <returns><c>true</c> se a atualização foi realizada com sucesso; caso contrário, <c>false</c>.</returns>
        Task<bool> AtualizarAsync(Cliente cliente);

        /// <summary>
        /// Remove um cliente pelo seu ID.
        /// </summary>
        /// <param name="id">ID do cliente a ser removido.</param>
        /// <returns><c>true</c> se o cliente foi removido com sucesso; caso contrário, <c>false</c>.</returns>
        Task<bool> RemoverAsync(int id);
    }
}
