using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Repositório específico para operações relacionadas à entidade <see cref="Produto"/>.
    /// </summary>
    /// <remarks>
    /// Fornece métodos adicionais ao <see cref="IRepository{Produto}"/> para consultas
    /// especializadas, como obtenção de produtos disponíveis e carregamento completo
    /// de um produto com seus relacionamentos.
    /// </remarks>
    public interface IProdutoRepository : IRepository<Produto>
    {
        /// <summary>
        /// Obtém todos os produtos atualmente disponíveis para venda.
        /// </summary>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> que, ao completar, contém uma coleção de
        /// <see cref="Produto"/> marcados como disponíveis.  
        /// </returns>
        /// <remarks>
        /// A lógica de disponibilidade pode variar conforme a implementação — por exemplo,
        /// produtos com estoque maior que zero, produtos com flag "Ativo", etc.
        /// </remarks>
        Task<IEnumerable<Produto>> GetProdutosDisponiveisAsync();

        /// <summary>
        /// Obtém um produto específico com todos os seus relacionamentos carregados.
        /// </summary>
        /// <param name="id">O identificador único do produto desejado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo o <see cref="Produto"/> completo
        /// (incluindo categorias, opções adicionais, complementos, imagens ou outros
        /// relacionamentos definidos), ou <c>null</c> caso não seja encontrado.
        /// </returns>
        /// <remarks>
        /// Este método deve ser usado quando é necessário exibir ou manipular o produto
        /// com todos os seus dados relacionados, evitando múltiplas consultas adicionais.
        /// </remarks>
        Task<Produto?> GetProdutoCompletoAsync(int id);
    }
}
