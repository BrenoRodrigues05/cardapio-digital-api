using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Interface de serviço para gerenciamento de produtos do cardápio digital.
    /// </summary>
    /// <remarks>
    /// Fornece operações de negócio para CRUD de produtos, controle de disponibilidade,
    /// gerenciamento de estoque e consultas especializadas.
    /// </remarks>
    public interface IProdutoService
    {
        // ==================== CRUD Básico ====================

        /// <summary>
        /// Cria um novo produto no sistema.
        /// </summary>
        /// <param name="produto">Dados do produto a ser criado.</param>
        /// <returns>O produto criado com o ID gerado.</returns>
        Task<Produto> CriarProdutoAsync(Produto produto);

        /// <summary>
        /// Obtém um produto específico por ID.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <returns>O produto encontrado ou null.</returns>
        Task<Produto?> ObterProdutoPorIdAsync(int id);

        /// <summary>
        /// Obtém um produto com todos os relacionamentos carregados.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <returns>Produto completo incluindo Restaurante e ItensPedido.</returns>
        Task<Produto?> ObterProdutoCompletoAsync(int id);

        /// <summary>
        /// Obtém todos os produtos cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de todos os produtos.</returns>
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync();

        /// <summary>
        /// Atualiza as informações de um produto existente.
        /// </summary>
        /// <param name="produto">Dados atualizados do produto.</param>
        /// <returns>O produto atualizado.</returns>
        Task<Produto> AtualizarProdutoAsync(Produto produto);

        /// <summary>
        /// Remove um produto do sistema.
        /// </summary>
        /// <param name="id">Identificador do produto a ser removido.</param>
        /// <returns>True se removido com sucesso.</returns>
        Task<bool> DeletarProdutoAsync(int id);

        // ==================== Consultas por Restaurante ====================

        /// <summary>
        /// Obtém todos os produtos de um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">Identificador do restaurante.</param>
        /// <returns>Lista de produtos do restaurante.</returns>
        Task<IEnumerable<Produto>> ObterProdutosPorRestauranteAsync(int restauranteId);

        /// <summary>
        /// Obtém produtos disponíveis de um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">Identificador do restaurante.</param>
        /// <returns>Lista de produtos disponíveis.</returns>
        Task<IEnumerable<Produto>> ObterProdutosDisponiveisPorRestauranteAsync(int restauranteId);

        // ==================== Disponibilidade ====================

        /// <summary>
        /// Obtém todos os produtos marcados como disponíveis.
        /// </summary>
        /// <returns>Lista de produtos disponíveis.</returns>
        Task<IEnumerable<Produto>> ObterProdutosDisponiveisAsync();

        /// <summary>
        /// Altera o status de disponibilidade de um produto.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="disponibilidade">Novo status de disponibilidade.</param>
        /// <returns>True se alterado com sucesso.</returns>
        Task<bool> AlterarDisponibilidadeAsync(int id, bool disponibilidade);

        /// <summary>
        /// Verifica se um produto está disponível para venda.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <returns>True se disponível.</returns>
        Task<bool> VerificarDisponibilidadeAsync(int id);

        /// <summary>
        /// Obtém lista de produtos indisponíveis.
        /// </summary>
        /// <returns>Lista de produtos indisponíveis.</returns>
        Task<IEnumerable<Produto>> ObterProdutosIndisponiveisAsync();

        // ==================== Gerenciamento de Estoque ====================

        /// <summary>
        /// Atualiza a quantidade em estoque de um produto.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="quantidade">Nova quantidade em estoque.</param>
        /// <returns>True se atualizado com sucesso.</returns>
        Task<bool> AtualizarEstoqueAsync(int id, int quantidade);

        /// <summary>
        /// Incrementa o estoque de um produto.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="quantidade">Quantidade a adicionar.</param>
        /// <returns>True se incrementado com sucesso.</returns>
        Task<bool> AdicionarEstoqueAsync(int id, int quantidade);

        /// <summary>
        /// Decrementa o estoque de um produto.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="quantidade">Quantidade a remover.</param>
        /// <returns>True se decrementado com sucesso.</returns>
        Task<bool> RemoverEstoqueAsync(int id, int quantidade);

        /// <summary>
        /// Obtém a quantidade atual em estoque.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <returns>Quantidade em estoque.</returns>
        Task<int> ObterQuantidadeEstoqueAsync(int id);

        /// <summary>
        /// Obtém produtos com estoque baixo (abaixo de um limite).
        /// </summary>
        /// <param name="limiteMinimo">Quantidade mínima para considerar estoque baixo.</param>
        /// <returns>Lista de produtos com estoque baixo.</returns>
        Task<IEnumerable<Produto>> ObterProdutosComEstoqueBaixoAsync(int limiteMinimo = 5);

        // ==================== Preços ====================

        /// <summary>
        /// Atualiza o preço de um produto.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="novoPreco">Novo preço do produto.</param>
        /// <returns>True se atualizado com sucesso.</returns>
        Task<bool> AtualizarPrecoAsync(int id, decimal novoPreco);

        /// <summary>
        /// Obtém produtos em uma faixa de preço específica.
        /// </summary>
        /// <param name="precoMin">Preço mínimo.</param>
        /// <param name="precoMax">Preço máximo.</param>
        /// <returns>Lista de produtos na faixa de preço.</returns>
        Task<IEnumerable<Produto>> ObterProdutosPorFaixaDePrecoAsync(decimal precoMin, decimal precoMax);

        // ==================== Busca e Filtros ====================

        /// <summary>
        /// Busca produtos por nome (busca parcial, case-insensitive).
        /// </summary>
        /// <param name="nome">Termo de busca.</param>
        /// <returns>Lista de produtos que correspondem ao termo.</returns>
        Task<IEnumerable<Produto>> BuscarProdutosPorNomeAsync(string nome);

        // ==================== Estatísticas ====================

        /// <summary>
        /// Obtém o total de produtos disponíveis.
        /// </summary>
        /// <returns>Quantidade de produtos disponíveis.</returns>
        Task<int> ObterTotalProdutosDisponiveisAsync();

        /// <summary>
        /// Calcula o valor total do estoque de produtos.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante (opcional).</param>
        /// <returns>Valor total em estoque.</returns>
        Task<decimal> CalcularValorTotalEstoqueAsync(int? restauranteId = null);

        // ==================== Validações ====================

        /// <summary>
        /// Verifica se um produto existe no sistema.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <returns>True se existe.</returns>
        Task<bool> ProdutoExisteAsync(int id);

        /// <summary>
        /// Verifica se já existe produto com o mesmo nome para um restaurante.
        /// </summary>
        /// <param name="nome">Nome do produto.</param>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <param name="produtoId">ID do produto atual (para edição).</param>
        /// <returns>True se já existe.</returns>
        Task<bool> ProdutoComNomeExisteAsync(string nome, int restauranteId, int? produtoId = null);
    }

}