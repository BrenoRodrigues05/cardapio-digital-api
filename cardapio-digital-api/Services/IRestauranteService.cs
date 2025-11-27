using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Interface que define os serviços relacionados a restaurantes.
    /// </summary>
    /// <remarks>
    /// Fornece operações CRUD, buscas por nome/endereço, verificações de existência e estatísticas de pedidos/produtos.
    /// </remarks>
    public interface IRestauranteService
    {
        /// <summary>
        /// Obtém todos os restaurantes cadastrados.
        /// </summary>
        /// <returns>Uma coleção de <see cref="Restaurante"/>.</returns>
        Task<IEnumerable<Restaurante>> ObterTodosAsync();

        /// <summary>
        /// Obtém um restaurante pelo seu ID.
        /// </summary>
        /// <param name="id">ID do restaurante.</param>
        /// <returns>O <see cref="Restaurante"/> correspondente ou <c>null</c> se não encontrado.</returns>
        Task<Restaurante?> ObterPorIdAsync(int id);

        /// <summary>
        /// Cria um novo restaurante no sistema.
        /// </summary>
        /// <param name="restaurante">Objeto <see cref="Restaurante"/> a ser criado.</param>
        /// <returns>O <see cref="Restaurante"/> criado com o ID gerado.</returns>
        Task<Restaurante> CriarAsync(Restaurante restaurante);

        /// <summary>
        /// Atualiza os dados de um restaurante existente.
        /// </summary>
        /// <param name="restaurante">Objeto <see cref="Restaurante"/> com dados atualizados.</param>
        /// <returns><c>true</c> se a atualização foi realizada com sucesso; caso contrário, <c>false</c>.</returns>
        Task<bool> AtualizarAsync(Restaurante restaurante);

        /// <summary>
        /// Remove um restaurante pelo seu ID.
        /// </summary>
        /// <param name="id">ID do restaurante a ser removido.</param>
        /// <returns><c>true</c> se o restaurante foi removido com sucesso; caso contrário, <c>false</c>.</returns>
        Task<bool> RemoverAsync(int id);

        /// <summary>
        /// Busca restaurantes pelo nome.
        /// </summary>
        /// <param name="nome">Nome ou parte do nome do restaurante.</param>
        /// <returns>Uma coleção de <see cref="Restaurante"/> que correspondem ao critério de busca.</returns>
        Task<IEnumerable<Restaurante>> BuscarPorNomeAsync(string nome);

        /// <summary>
        /// Busca restaurantes pelo endereço.
        /// </summary>
        /// <param name="endereco">Endereço ou parte do endereço do restaurante.</param>
        /// <returns>Uma coleção de <see cref="Restaurante"/> que correspondem ao critério de busca.</returns>
        Task<IEnumerable<Restaurante>> BuscarPorEnderecoAsync(string endereco);

        /// <summary>
        /// Verifica se um restaurante existe pelo seu ID.
        /// </summary>
        /// <param name="id">ID do restaurante.</param>
        /// <returns><c>true</c> se o restaurante existe; caso contrário, <c>false</c>.</returns>
        Task<bool> RestauranteExisteAsync(int id);

        /// <summary>
        /// Verifica se já existe um restaurante com o mesmo nome.
        /// </summary>
        /// <param name="nome">Nome do restaurante.</param>
        /// <param name="restauranteId">ID do restaurante a ser ignorado (opcional, usado para atualização).</param>
        /// <returns><c>true</c> se existir outro restaurante com o mesmo nome; caso contrário, <c>false</c>.</returns>
        Task<bool> RestauranteComNomeExisteAsync(string nome, int? restauranteId = null);

        /// <summary>
        /// Obtém o total de pedidos realizados em um restaurante.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>O total de pedidos como <see cref="int"/>.</returns>
        Task<int> ObterTotalPedidosAsync(int restauranteId);

        /// <summary>
        /// Obtém o total de produtos cadastrados em um restaurante.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>O total de produtos como <see cref="int"/>.</returns>
        Task<int> ObterTotalProdutosAsync(int restauranteId);
    }
}
