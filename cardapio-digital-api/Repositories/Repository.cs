using cardapio_digital_api.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Implementação genérica do repositório para operações básicas de acesso a dados.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade associada ao repositório. Deve ser uma classe.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Contexto do banco de dados utilizado pelo repositório.
        /// </summary>
        protected CardapioDigitalDbContext _ctx;

        /// <summary>
        /// Conjunto de entidades do tipo <typeparamref name="T"/> gerenciado pelo Entity Framework.
        /// </summary>
        protected DbSet<T> _dbSet;

        /// <summary>
        /// Inicializa uma nova instância do repositório genérico.
        /// </summary>
        /// <param name="ctx">Contexto do banco de dados.</param>
        public Repository(CardapioDigitalDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<T>();
        }

        /// <summary>
        /// Retorna todas as entidades do tipo <typeparamref name="T"/> do banco de dados.
        /// </summary>
        /// <returns>Uma coleção enumerável contendo todas as entidades encontradas.</returns>
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        /// <summary>
        /// Retorna uma entidade específica com base no identificador informado.
        /// </summary>
        /// <param name="id">Identificador único da entidade.</param>
        /// <returns>A entidade correspondente ao identificador, ou <c>null</c> se não for encontrada.</returns>
        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        /// <summary>
        /// Retorna uma entidade específica que atende à expressão informada.
        /// </summary>
        /// <param name="predicate">Expressão que define a condição de busca.</param>
        /// <returns>A entidade correspondente ou <c>null</c> se não for encontrada.</returns>
        public async Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            return await _ctx.Set<T>()
            .Where(predicate)
             .ToListAsync();
        }

        /// <summary>
        /// Adiciona uma nova entidade do tipo <typeparamref name="T"/> ao contexto.
        /// </summary>
        /// <param name="entity">A instância da entidade a ser adicionada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona de adição.</returns>
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        /// <summary>
        /// Atualiza os dados de uma entidade existente no contexto.
        /// </summary>
        /// <param name="entity">A instância da entidade a ser atualizada.</param>
        public async Task Update(T entity) => _dbSet.Update(entity);

        /// <summary>
        /// Remove uma entidade existente do contexto.
        /// </summary>
        /// <param name="entity">A instância da entidade a ser removida.</param>
        public void Remove(T entity) => _dbSet.Remove(entity);

       public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _ctx.Set<T>()
        .AsNoTracking()
        .FirstOrDefaultAsync(predicate);
        }
    }
}
