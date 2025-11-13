using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Implementação do repositório específico para a entidade <see cref="Produto"/>.
    /// </summary>
    /// <remarks>
    /// Fornece métodos especializados para recuperar produtos com base em suas regras
    /// de disponibilidade e para carregar informações completas relacionadas.
    /// </remarks>
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="ProdutoRepository"/>.
        /// </summary>
        /// <param name="ctx">Contexto do banco de dados utilizado para acessar as entidades.</param>
        public ProdutoRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Obtém todos os produtos que estão marcados como disponíveis para venda.
        /// </summary>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo uma coleção de <see cref="Produto"/>
        /// cuja propriedade <c>Disponivel</c> é verdadeira.
        /// </returns>
        /// <remarks>
        /// A lógica de disponibilidade baseia-se no campo <c>Disponivel</c> da entidade.
        /// Implementações alternativas podem incluir outras regras futuramente, como
        /// validação de estoque mínimo ou períodos de disponibilidade.
        /// </remarks>
        public async Task<IEnumerable<Produto>> GetProdutosDisponiveisAsync()
        {
            return await _dbSet
                .Where(p => p.Disponivel)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um produto específico com todos os seus relacionamentos carregados.
        /// </summary>
        /// <param name="id">Identificador único do produto desejado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo o <see cref="Produto"/> com seus dados
        /// completos (incluindo o restaurante e itens de pedidos relacionados),
        /// ou <c>null</c> se nenhum registro for encontrado.
        /// </returns>
        /// <remarks>
        /// Ideal para cenários em que é necessário exibir detalhes do produto juntamente
        /// com suas referências, evitando múltiplas consultas adicionais ao banco.
        /// </remarks>
        public async Task<Produto?> GetProdutoCompletoAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Restaurante)
                .Include(p => p.ItensPedido)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
