using System.Linq.Expressions;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Define um contrato genérico para operações básicas de acesso a dados.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo da entidade associada ao repositório. Deve ser uma classe.
    /// </typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retorna todas as entidades do tipo <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// Uma coleção enumerável contendo todas as entidades encontradas.
        /// </returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retorna uma entidade específica com base no identificador informado.
        /// </summary>
        /// <param name="id">
        /// Identificador único da entidade.
        /// </param>
        /// <returns>
        /// A entidade correspondente ao identificador, ou <c>null</c> se não for encontrada.
        /// </returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Retorna uma entidade específica baseada em um campo string.
        /// </summary>
        /// <param name="predicate">Expressão que define a condição de busca.</param>
        /// <returns>A entidade correspondente ou <c>null</c> se não for encontrada.</returns>
        Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adiciona uma nova entidade do tipo <typeparamref name="T"/> ao contexto.
        /// </summary>
        /// <param name="entity">
        /// A instância da entidade a ser adicionada.
        /// </param>
        /// <returns>
        /// Uma tarefa que representa a operação assíncrona de adição.
        /// </returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Atualiza os dados de uma entidade existente no contexto.
        /// </summary>
        /// <param name="entity">
        /// A instância da entidade a ser atualizada.
        /// </param>
        void Update(T entity);

        /// <summary>
        /// Remove uma entidade existente do contexto.
        /// </summary>
        /// <param name="entity">
        /// A instância da entidade a ser removida.
        /// </param>
        void Remove(T entity);
    }
}
